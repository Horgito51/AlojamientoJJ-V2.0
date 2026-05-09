using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Reservas;
using Servicio.Hotel.Business.DTOs.Seguridad;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.Interfaces.Reservas;
using Servicio.Hotel.Business.Interfaces.Seguridad;
using Servicio.Hotel.Business.Validators.Seguridad;
using Servicio.Hotel.DataManagement.Seguridad.Interfaces;
using Servicio.Hotel.DataManagement.Seguridad.Models;

namespace Servicio.Hotel.Business.Services.Seguridad
{
    public class AuthService : IAuthService
    {
        private static readonly ConcurrentDictionary<string, DateTime> RevokedRefreshTokens = new();
        private readonly IUsuarioDataService _usuarioDataService;
        private readonly IConfiguration _configuration;
        private readonly IRolDataService _rolDataService;
        private readonly IClienteService _clienteService;

        public AuthService(IUsuarioDataService usuarioDataService, IConfiguration configuration, IRolDataService rolDataService, IClienteService clienteService)
        {
            _usuarioDataService = usuarioDataService;
            _configuration = configuration;
            _rolDataService = rolDataService;
            _clienteService = clienteService;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequest, CancellationToken ct = default)
        {
            LoginValidator.Validate(loginRequest);

            var loginKey = loginRequest.Username.Trim();

            var credenciales = await _usuarioDataService.GetCredentialsByUsernameAsync(loginKey, ct);
            var usuario = await _usuarioDataService.GetByUsernameAsync(loginKey, ct);

            if (credenciales == null || usuario == null)
            {
                credenciales = await _usuarioDataService.GetCredentialsByCorreoAsync(loginKey, ct);
                usuario = await _usuarioDataService.GetByCorreoAsync(loginKey, ct);
            }

            if (credenciales == null || usuario == null)
                throw new UnauthorizedBusinessException("AUTH-001", "Usuario o contraseña incorrectos.");

            if (!usuario.Activo || usuario.EstadoUsuario != "ACT" || usuario.EsEliminado)
                throw new UnauthorizedBusinessException("AUTH-001", "Usuario o contraseña incorrectos.");

            if (!PasswordHasher.Verify(loginRequest.Password, credenciales.PasswordHash, credenciales.PasswordSalt))
                throw new UnauthorizedBusinessException("AUTH-001", "Usuario o contraseña incorrectos.");

            var roles = usuario.Roles?.Select(r => r.NombreRol).ToList() ?? new List<string>();
            var token = GenerarTokenJWT(usuario, roles);
            var expirationMinutes = int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var mins) ? mins : 60;

            var refreshToken = GenerarRefreshTokenJWT(usuario);

            return new LoginResponseDTO
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                ExpiresIn = expirationMinutes * 60,
                UsuarioGuid = usuario.UsuarioGuid,
                IdCliente = usuario.IdCliente,
                ClienteGuid = await GetClienteGuidAsync(usuario.IdCliente, ct),
                Username = usuario.Username,
                Correo = usuario.Correo,
                NombreCompleto = $"{usuario.Nombres} {usuario.Apellidos}",
                Roles = roles
            };
        }

        private string GenerarTokenJWT(UsuarioDataModel usuario, List<string> roles)
        {
            // Leer exactamente los mismos valores que usa AuthenticationExtensions
            var secret   = GetRequiredJwtSetting("Secret");
            var issuer   = GetRequiredJwtSetting("Issuer");
            var audience = GetRequiredJwtSetting("Audience");
            var expirationMinutes = int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var mins) ? mins : 60;

            var key = Encoding.ASCII.GetBytes(secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim("usuarioGuid", usuario.UsuarioGuid.ToString()),
                new Claim("nombres", usuario.Nombres ?? ""),
                new Claim("apellidos", usuario.Apellidos ?? "")
            };

            if (usuario.IdCliente.HasValue)
                claims.Add(new Claim("idCliente", usuario.IdCliente.Value.ToString()));

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        private string GenerarRefreshTokenJWT(UsuarioDataModel usuario)
        {
            var secret = GetRequiredJwtSetting("Secret");
            var issuer = GetRequiredJwtSetting("Issuer");
            var audience = GetRequiredJwtSetting("Audience");
            var refreshDays = int.TryParse(_configuration["Jwt:RefreshExpirationDays"], out var days) ? days : 7;

            var key = Encoding.ASCII.GetBytes(secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim("token_type", "refresh")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(refreshDays),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        public Task LogoutAsync(string refreshToken, CancellationToken ct = default)
        {
            RevokeRefreshToken(refreshToken);
            return Task.CompletedTask;
        }

        public async Task<LoginResponseDTO> RefreshTokenAsync(string refreshToken, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new UnauthorizedBusinessException("AUTH-002", "Refresh token inválido.");

            if (IsRefreshTokenRevoked(refreshToken))
                throw new UnauthorizedBusinessException("AUTH-002", "Refresh token inválido.");

            var principal = ValidateJwt(refreshToken, out var tokenType);
            if (principal == null || tokenType != "refresh")
                throw new UnauthorizedBusinessException("AUTH-002", "Refresh token inválido.");

            var idValue = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(idValue, out var idUsuario))
                throw new UnauthorizedBusinessException("AUTH-002", "Refresh token inválido.");

            var usuario = await _usuarioDataService.GetByIdAsync(idUsuario, ct);
            if (usuario == null)
                throw new UnauthorizedBusinessException("AUTH-002", "Refresh token inválido.");

            if (!usuario.Activo || usuario.EstadoUsuario != "ACT" || usuario.EsEliminado)
                throw new UnauthorizedBusinessException("AUTH-002", "Refresh token inválido.");

            var roles = usuario.Roles?.Select(r => r.NombreRol).ToList() ?? new List<string>();
            var token = GenerarTokenJWT(usuario, roles);
            var newRefresh = GenerarRefreshTokenJWT(usuario);
            RevokeRefreshToken(refreshToken);
            var expirationMinutes = int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var mins) ? mins : 60;

            return new LoginResponseDTO
            {
                AccessToken = token,
                RefreshToken = newRefresh,
                ExpiresIn = expirationMinutes * 60,
                UsuarioGuid = usuario.UsuarioGuid,
                IdCliente = usuario.IdCliente,
                ClienteGuid = await GetClienteGuidAsync(usuario.IdCliente, ct),
                Username = usuario.Username,
                Correo = usuario.Correo,
                NombreCompleto = $"{usuario.Nombres} {usuario.Apellidos}",
                Roles = roles
            };
        }

        public async Task CambiarPasswordAsync(int idUsuario, string passwordActual, string passwordNuevo, string usuario, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(passwordActual) || string.IsNullOrWhiteSpace(passwordNuevo))
                throw new ValidationException("AUTH-003", "password_actual y password_nuevo son obligatorios.");

            if (passwordNuevo.Length < 10)
                throw new ValidationException("AUTH-006", "La nueva contraseña debe tener al menos 10 caracteres.");

            var cred = await _usuarioDataService.GetCredentialsByIdAsync(idUsuario, ct);
            if (cred == null)
                throw new NotFoundException("AUTH-004", "Usuario no encontrado.");

            if (!PasswordHasher.Verify(passwordActual, cred.PasswordHash, cred.PasswordSalt))
                throw new UnauthorizedBusinessException("AUTH-005", "La contraseña actual no es correcta.");

            var (hash, salt) = PasswordHasher.HashPassword(passwordNuevo);
            await _usuarioDataService.CambiarPasswordAsync(idUsuario, hash, salt, usuario, ct);
        }

        private ClaimsPrincipal? ValidateJwt(string token, out string? tokenType)
        {
            tokenType = null;
            var secret = GetRequiredJwtSetting("Secret");
            var issuer = GetRequiredJwtSetting("Issuer");
            var audience = GetRequiredJwtSetting("Audience");

            var key = Encoding.ASCII.GetBytes(secret);
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                tokenType = principal.FindFirst("token_type")?.Value;
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public Task<bool> ValidateTokenAsync(string token, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                return Task.FromResult(false);

            var principal = ValidateJwt(token, out var tokenType);
            return Task.FromResult(principal != null && tokenType != "refresh");
        }

        private string GetRequiredJwtSetting(string name)
        {
            var value = _configuration[$"Jwt:{name}"];
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException($"La configuracion 'Jwt:{name}' es obligatoria.");

            return value;
        }

        private static void RevokeRefreshToken(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return;

            var expiresUtc = GetTokenExpirationUtc(refreshToken) ?? DateTime.UtcNow.AddDays(7);
            RevokedRefreshTokens[HashToken(refreshToken)] = expiresUtc;
            CleanupRevokedRefreshTokens();
        }

        public async Task<LoginResponseDTO> RegisterClienteAsync(RegisterClienteDTO registerRequest, CancellationToken ct = default)
        {
            // Validar que las contraseñas coincidan
            if (registerRequest.Password != registerRequest.ConfirmPassword)
                throw new ValidationException("AUTH-007", "Las contraseñas no coinciden.");

            // Validar datos básicos
            if (string.IsNullOrWhiteSpace(registerRequest.Username))
                throw new ValidationException("AUTH-008", "El nombre de usuario es obligatorio.");
            if (string.IsNullOrWhiteSpace(registerRequest.Correo))
                throw new ValidationException("AUTH-009", "El correo es obligatorio.");
            if (string.IsNullOrWhiteSpace(registerRequest.Nombres))
                throw new ValidationException("AUTH-010", "El nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(registerRequest.Password) || registerRequest.Password.Length < 10)
                throw new ValidationException("AUTH-011", "La contraseña debe tener al menos 10 caracteres.");

            // Verificar que el usuario no exista
            var existsByUsername = await _usuarioDataService.ExistsByUsernameAsync(registerRequest.Username, null, ct);
            if (existsByUsername)
                throw new BusinessException("AUTH-012", "El nombre de usuario ya está registrado.");

            var existsByEmail = await _usuarioDataService.ExistsByCorreoAsync(registerRequest.Correo, null, ct);
            if (existsByEmail)
                throw new BusinessException("AUTH-013", "El correo ya está registrado.");

            // 🔥 Obtener el rol CLIENTE (id = 4 - QUEMADO)
            var clienteRole = await GetClienteRoleAsync(ct);
            if (clienteRole == null)
                throw new NotFoundException("AUTH-014", "No se encontró el rol de cliente en el sistema.");

            // Hash de contraseña
            var (hash, salt) = PasswordHasher.HashPassword(registerRequest.Password);

            var cliente = await _clienteService.CreateAsync(new ClienteCreateDTO
            {
                TipoIdentificacion = "CLI",
                NumeroIdentificacion = $"CLI-{Guid.NewGuid():N}"[..20],
                Nombres = registerRequest.Nombres.Trim(),
                Apellidos = registerRequest.Apellidos?.Trim() ?? "",
                RazonSocial = "",
                Correo = registerRequest.Correo.Trim(),
                Telefono = "0000000000",
                Direccion = "",
                Estado = "ACT"
            }, ct);

            // Crear el usuario público
            var nuevoUsuario = new Servicio.Hotel.DataManagement.Seguridad.Models.UsuarioDataModel
            {
                IdCliente = cliente.IdCliente,
                Username = registerRequest.Username.Trim(),
                Correo = registerRequest.Correo.Trim(),
                Nombres = registerRequest.Nombres.Trim(),
                Apellidos = registerRequest.Apellidos?.Trim() ?? "",
                PasswordHash = hash,
                PasswordSalt = salt,
                EstadoUsuario = "ACT",
                Activo = true,
                EsEliminado = false,
                Roles = new() { clienteRole }
            };

            var usuarioCreado = await _usuarioDataService.AddAsync(nuevoUsuario, ct);
            usuarioCreado = await _usuarioDataService.GetByIdAsync(usuarioCreado.IdUsuario, ct) ?? usuarioCreado;

            // Generar tokens
            var roles = usuarioCreado.Roles?.Select(r => r.NombreRol).ToList() ?? new List<string>();
            var token = GenerarTokenJWT(usuarioCreado, roles);
            var refreshToken = GenerarRefreshTokenJWT(usuarioCreado);
            var expirationMinutes = int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var mins) ? mins : 60;

            return new LoginResponseDTO
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                ExpiresIn = expirationMinutes * 60,
                UsuarioGuid = usuarioCreado.UsuarioGuid,
                IdCliente = usuarioCreado.IdCliente,
                ClienteGuid = cliente.ClienteGuid,
                Username = usuarioCreado.Username,
                Correo = usuarioCreado.Correo,
                NombreCompleto = $"{usuarioCreado.Nombres} {usuarioCreado.Apellidos}".Trim(),
                Roles = roles
            };
        }

        private async Task<Guid?> GetClienteGuidAsync(int? idCliente, CancellationToken ct)
        {
            if (!idCliente.HasValue || idCliente.Value <= 0)
                return null;

            try
            {
                var cliente = await _clienteService.GetByIdAsync(idCliente.Value, ct);
                return cliente.ClienteGuid;
            }
            catch
            {
                return null;
            }
        }

        private async Task<Servicio.Hotel.DataManagement.Seguridad.Models.RolDataModel> GetClienteRoleAsync(CancellationToken ct)
        {
            try
            {
                // 🔥 El rol CLIENTE SIEMPRE tiene id = 4
                const int CLIENTE_ROLE_ID = 4;
                return await _rolDataService.GetByIdAsync(CLIENTE_ROLE_ID, ct);
            }
            catch
            {
                return null;
            }
        }

        private static bool IsRefreshTokenRevoked(string refreshToken)
        {
            CleanupRevokedRefreshTokens();
            return RevokedRefreshTokens.ContainsKey(HashToken(refreshToken));
        }

        private static DateTime? GetTokenExpirationUtc(string token)
        {
            try
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                return jwt.ValidTo == DateTime.MinValue ? null : jwt.ValidTo;
            }
            catch
            {
                return null;
            }
        }

        private static string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes);
        }

        private static void CleanupRevokedRefreshTokens()
        {
            var now = DateTime.UtcNow;
            foreach (var item in RevokedRefreshTokens)
            {
                if (item.Value <= now)
                    RevokedRefreshTokens.TryRemove(item.Key, out _);
            }
        }
    }
}

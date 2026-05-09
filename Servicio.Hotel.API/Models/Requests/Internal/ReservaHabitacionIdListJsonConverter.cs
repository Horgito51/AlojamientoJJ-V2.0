using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Servicio.Hotel.API.Models.Requests.Internal
{
    // Soporta ambos formatos:
    // - "habitaciones": [1,2]
    // - "habitaciones": [{"idHabitacion": 1}, {"idHabitacion": 2}]
    public sealed class ReservaHabitacionIdListJsonConverter : JsonConverter<List<ReservaHabitacionIdRequest>>
    {
        public override List<ReservaHabitacionIdRequest>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return new List<ReservaHabitacionIdRequest>();

            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException("El campo 'habitaciones' debe ser un arreglo.");

            var result = new List<ReservaHabitacionIdRequest>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    return result;

                if (reader.TokenType == JsonTokenType.Number)
                {
                    var id = reader.GetInt32();
                    result.Add(new ReservaHabitacionIdRequest { IdHabitacion = id });
                    continue;
                }

                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    int? idHabitacion = null;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject)
                            break;

                        if (reader.TokenType != JsonTokenType.PropertyName)
                            throw new JsonException("Formato inválido en 'habitaciones'.");

                        var propName = reader.GetString() ?? string.Empty;
                        reader.Read();

                        if (string.Equals(propName, "idHabitacion", StringComparison.OrdinalIgnoreCase))
                        {
                            if (reader.TokenType != JsonTokenType.Number)
                                throw new JsonException("El campo 'idHabitacion' debe ser numérico.");
                            idHabitacion = reader.GetInt32();
                        }
                        else
                        {
                            // Ignorar propiedades desconocidas
                            using var _ = JsonDocument.ParseValue(ref reader);
                        }
                    }

                    if (!idHabitacion.HasValue)
                        throw new JsonException("Cada objeto en 'habitaciones' debe incluir 'idHabitacion'.");

                    result.Add(new ReservaHabitacionIdRequest { IdHabitacion = idHabitacion.Value });
                    continue;
                }

                throw new JsonException("Formato inválido en 'habitaciones'.");
            }

            throw new JsonException("JSON incompleto en 'habitaciones'.");
        }

        public override void Write(Utf8JsonWriter writer, List<ReservaHabitacionIdRequest> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var item in value)
            {
                writer.WriteStartObject();
                writer.WriteNumber("idHabitacion", item.IdHabitacion);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
    }
}


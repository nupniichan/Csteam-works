using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Text.Json;
using csteamworks.Models.App.Components;

namespace csteamworks.Models.App
{
    [JsonConverter(typeof(PCRequirementsConverter))]
    public class PCRequirements
    {
        public string minimum { get; set; }
        public string recommended { get; set; }
        
        [JsonIgnore]
        public SystemRequirementsDetail MinimumRequirements => ParseSystemRequirements(minimum);
        
        [JsonIgnore]
        public SystemRequirementsDetail RecommendedRequirements => ParseSystemRequirements(recommended);
        
        private SystemRequirementsDetail ParseSystemRequirements(string htmlContent)
        {
            if (string.IsNullOrEmpty(htmlContent))
                return new SystemRequirementsDetail();
                
            var result = new SystemRequirementsDetail();
            
            if (htmlContent.Contains("<strong>OS:</strong>"))
            {
                var osStart = htmlContent.IndexOf("<strong>OS:</strong>") + "<strong>OS:</strong>".Length;
                var osEnd = htmlContent.IndexOf("<br>", osStart);
                if (osEnd > osStart)
                {
                    result.OS = htmlContent.Substring(osStart, osEnd - osStart).Trim();
                }
            }
            
            if (htmlContent.Contains("<strong>Processor:</strong>"))
            {
                var processorStart = htmlContent.IndexOf("<strong>Processor:</strong>") + "<strong>Processor:</strong>".Length;
                var processorEnd = htmlContent.IndexOf("<br>", processorStart);
                if (processorEnd > processorStart)
                {
                    result.Processor = htmlContent.Substring(processorStart, processorEnd - processorStart).Trim();
                }
            }
            
            if (htmlContent.Contains("<strong>Memory:</strong>"))
            {
                var memoryStart = htmlContent.IndexOf("<strong>Memory:</strong>") + "<strong>Memory:</strong>".Length;
                var memoryEnd = htmlContent.IndexOf("<br>", memoryStart);
                if (memoryEnd > memoryStart)
                {
                    result.Memory = htmlContent.Substring(memoryStart, memoryEnd - memoryStart).Trim();
                }
            }
            
            if (htmlContent.Contains("<strong>Graphics:</strong>"))
            {
                var graphicsStart = htmlContent.IndexOf("<strong>Graphics:</strong>") + "<strong>Graphics:</strong>".Length;
                var graphicsEnd = htmlContent.IndexOf("<br>", graphicsStart);
                if (graphicsEnd > graphicsStart)
                {
                    result.Graphics = htmlContent.Substring(graphicsStart, graphicsEnd - graphicsStart).Trim();
                }
            }
            
            if (htmlContent.Contains("<strong>DirectX:</strong>"))
            {
                var dxStart = htmlContent.IndexOf("<strong>DirectX:</strong>") + "<strong>DirectX:</strong>".Length;
                var dxEnd = htmlContent.IndexOf("<br>", dxStart);
                if (dxEnd > dxStart)
                {
                    result.DirectX = htmlContent.Substring(dxStart, dxEnd - dxStart).Trim();
                }
            }
            
            if (htmlContent.Contains("<strong>Storage:</strong>"))
            {
                var storageStart = htmlContent.IndexOf("<strong>Storage:</strong>") + "<strong>Storage:</strong>".Length;
                var storageEnd = htmlContent.IndexOf("<br>", storageStart);
                if (storageEnd > storageStart)
                {
                    result.Storage = htmlContent.Substring(storageStart, storageEnd - storageStart).Trim();
                }
            }
            
            if (htmlContent.Contains("<strong>Network:</strong>"))
            {
                var networkStart = htmlContent.IndexOf("<strong>Network:</strong>") + "<strong>Network:</strong>".Length;
                var networkEnd = htmlContent.IndexOf("<br>", networkStart);
                if (networkEnd > networkStart)
                {
                    result.Network = htmlContent.Substring(networkStart, networkEnd - networkStart).Trim();
                }
            }
            
            if (htmlContent.Contains("<strong>Sound Card:</strong>"))
            {
                var soundStart = htmlContent.IndexOf("<strong>Sound Card:</strong>") + "<strong>Sound Card:</strong>".Length;
                var soundEnd = htmlContent.IndexOf("<br>", soundStart);
                if (soundEnd > soundStart)
                {
                    result.SoundCard = htmlContent.Substring(soundStart, soundEnd - soundStart).Trim();
                }
            }
            
            if (htmlContent.Contains("<strong>Additional Notes:</strong>"))
            {
                var notesStart = htmlContent.IndexOf("<strong>Additional Notes:</strong>") + "<strong>Additional Notes:</strong>".Length;
                var notesEnd = htmlContent.IndexOf("<br>", notesStart);
                if (notesEnd > notesStart)
                {
                    result.AdditionalNotes = htmlContent.Substring(notesStart, notesEnd - notesStart).Trim();
                }
            }
            
            return result;
        }
    }
    
    public class PCRequirementsConverter : JsonConverter<PCRequirements>
    {
        public override PCRequirements Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var depth = 1;
                while (depth > 0 && reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.StartArray)
                        depth++;
                    else if (reader.TokenType == JsonTokenType.EndArray)
                        depth--;
                }
                
                return new PCRequirements();
            }
            
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                var requirements = new PCRequirements();
                
                while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                {
                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        string propertyName = reader.GetString();
                        reader.Read();
                        
                        switch (propertyName)
                        {
                            case "minimum":
                                requirements.minimum = reader.GetString();
                                break;
                            case "recommended":
                                requirements.recommended = reader.GetString();
                                break;
                            default:
                                JsonDocument.ParseValue(ref reader);
                                break;
                        }
                    }
                }
                
                return requirements;
            }
            
            return new PCRequirements();
        }
        
        public override void Write(Utf8JsonWriter writer, PCRequirements value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            
            if (!string.IsNullOrEmpty(value.minimum))
            {
                writer.WriteString("minimum", value.minimum);
            }
            
            if (!string.IsNullOrEmpty(value.recommended))
            {
                writer.WriteString("recommended", value.recommended);
            }
            
            writer.WriteEndObject();
        }
    }
}
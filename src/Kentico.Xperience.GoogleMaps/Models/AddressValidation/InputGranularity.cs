using System.Text.Json.Serialization;

namespace Kentico.Xperience.GoogleMaps
{
    /// <summary>
    /// Represents the input granularity for address validation.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum InputGranularity
    {
        /// <summary>
        /// Represents a premise level input granularity.
        /// </summary>
        [JsonPropertyName("PREMISE")]
        Premise,


        /// <summary>
        /// Represents a sub-premise level input granularity.
        /// </summary>
        [JsonPropertyName("SUB_PREMISE")]
        SubPremise,


        /// <summary>
        /// Represents a premise proximity level input granularity.
        /// </summary>
        [JsonPropertyName("PREMISE_PROXIMITY")]
        Premise_Proximity,


        /// <summary>
        /// Represents a block level input granularity.
        /// </summary>
        [JsonPropertyName("BLOCK")]
        Block,


        /// <summary>
        /// Represents a route level input granularity.
        /// </summary>
        [JsonPropertyName("ROUTE")]
        Route,


        /// <summary>
        /// Represents an other level input granularity.
        /// </summary>
        [JsonPropertyName("OTHER")]
        Other,


        /// <summary>
        /// Represents an unspecified input granularity.
        /// </summary>
        [JsonPropertyName("GRANULARITY_UNSPECIFIED")]
        GranularityUnspecified
    }
}

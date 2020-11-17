﻿using System.Text.Json.Serialization;

namespace MtgApiManager.Lib.Service
{
    /// <summary>
    /// Query parameters for the <see cref="Model.Set"/> object.
    /// </summary>
    public class SetQueryParameter : QueryParameterBase
    {
        /// <summary>
        /// Gets or sets the block the set is in.
        /// </summary>
        [JsonPropertyName("block")]
        public string Block { get; set; }

        /// <summary>
        /// Gets or sets the name of the set.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
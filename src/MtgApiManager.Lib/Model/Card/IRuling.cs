﻿namespace MtgApiManager.Lib.Model
{
    /// <summary>
    /// Object representing a ruling for a card.
    /// </summary>
    public interface IRuling
    {
        /// <summary>
        /// Gets or sets the date of the ruling.
        /// </summary>
        string Date { get; }

        /// <summary>
        /// Gets or sets the text of the ruling.
        /// </summary>
        string Text { get; }
    }
}
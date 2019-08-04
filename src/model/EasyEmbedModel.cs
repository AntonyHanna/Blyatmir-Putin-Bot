using Discord;
using System.Collections.Generic;

namespace Blyatmir_Putin_Bot.Model
{
    public class EasyEmbed
    {
        private EmbedAuthorBuilder _author;
        private EmbedBuilder _embed;
        private EmbedFooterBuilder _footer;

        public string AuthorName { get; set; }
        public string AuthorIcon { get; set; }
        public Color EmbedColor { get; set; }
        public string EmbedImage { get; set; }
        public string EmbedThumbnail { get; set; }
        public string EmbedTitle { get; set; }
        public string EmbedDescription { get; set; }
        public EmbedFieldBuilder EmbedField { get; set; }
        public List<EmbedFieldBuilder> EmbedFieldList { get; set; }
        public string FooterIcon { get; set; }
        public string FooterText { get; set; }

        public EasyEmbed()
        {
            _author = new EmbedAuthorBuilder();
            _embed = new EmbedBuilder();
            _footer = new EmbedFooterBuilder();
        }

        /// <summary>
        /// Assign the values and return the embed
        /// </summary>
        /// <returns></returns>
        public Embed Build()
        {
            //attach the author and footer
            _embed.Author = _author;
            _embed.Footer = _footer;

            //FOR ALL THE BELOW
            //check if the value is null or default values
            //if they're not it'll assign the value

            if (!string.IsNullOrWhiteSpace(AuthorName))
                _author.Name = AuthorName;

            if (!string.IsNullOrWhiteSpace(AuthorIcon))
                _author.IconUrl = this.AuthorIcon;

            if (_embed.Color != Color.Default)
                _embed.Color = this.EmbedColor;

            if (!string.IsNullOrWhiteSpace(EmbedImage))
                _embed.ImageUrl = EmbedImage;

            if (!string.IsNullOrWhiteSpace(EmbedThumbnail))
                _embed.ThumbnailUrl = this.EmbedThumbnail;

            if (!string.IsNullOrWhiteSpace(EmbedTitle))
                _embed.Title = this.EmbedTitle;

            if (!string.IsNullOrWhiteSpace(EmbedDescription))
                _embed.Description = this.EmbedDescription;

            if (EmbedField != null)
                _embed.AddField(this.EmbedField);

            if (EmbedFieldList != null)
                foreach (var fields in EmbedFieldList)
                    _embed.AddField(fields);

            if (!string.IsNullOrWhiteSpace(FooterText))
                _footer.Text = this.FooterText;

            if (!string.IsNullOrWhiteSpace(FooterIcon))
                _footer.IconUrl = this.FooterIcon;

            //return the embed
            return _embed.Build();
        }

        //blank template below

        //var easyEmbed = new EasyEmbed()
        //{
        //    AuthorName = "",
        //    AuthorIcon = $"",
        //    EmbedColor = Color.Default,
        //    EmbedIcon  = $"",
        //    EmbedThumbnail = $"",
        //    EmbedTitle = $"",
        //    EmbedDescription = $"",
        //    EmbedField = null,
        //    FooterIcon = $"",
        //    FooterText = $""
        //};
    }
}

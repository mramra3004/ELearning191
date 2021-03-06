using System;
using MediatR;

namespace PeaLearning.Application.Commands.Blog
{
    public class UpdateBlogCommand : IRequest
    {
        public UpdateBlogCommand(Guid id, string name, string thumbnail, string description, bool? homeFlag,
            bool? hotFlag, string seoPageTitle, string seoAlias, string seoKeywords, string seoDescription, bool status,
            string tags, string content)
        {
            Id = id;
            Name = name;
            Thumbnail = thumbnail;
            Description = description;
            HomeFlag = homeFlag;
            HotFlag = hotFlag;
            SeoPageTitle = seoPageTitle;
            SeoAlias = seoAlias;
            SeoKeywords = seoKeywords;
            SeoDescription = seoDescription;
            Status = status;
            Tags = tags;
            Content = content;
        }

        public Guid Id { private set; get; }
        public string Name { private set; get; }
        public string Thumbnail { private set; get; }
        public string Description { private set; get; }
        public bool? HomeFlag { private set; get; }
        public bool? HotFlag { private set; get; }
        public string SeoPageTitle { private set; get; }
        public string SeoAlias { private set; get; }
        public string SeoKeywords { private set; get; }
        public string SeoDescription { private set; get; }
        public bool Status { private set; get; }
        public string Tags { get; private set; }
        public string Content { get; private set; }
    }
}
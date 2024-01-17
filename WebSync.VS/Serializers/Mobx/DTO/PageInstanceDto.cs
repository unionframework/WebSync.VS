using RoslynSpike.SessionWeb.Models;
using System;

namespace RoslynSpike.Ember.DTO
{
    public class PageInstanceDto : DtoBase
    {
        public String pageType;
        public String name;
        public String url;

        public PageInstanceDto(IPageType pageType) : base(pageType.Id)
        {
            this.pageType = pageType.Id;
            this.name = this.pageType.Substring(this.pageType.LastIndexOf('.'));
            this.url = "";
        }
    }
}
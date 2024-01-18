using RoslynSpike.SessionWeb.Models;
using System;

namespace RoslynSpike.Ember.DTO
{
    public class PageInstanceDto : DtoBase
    {
        public string pageType;
        public string name;
        public string url;

        public PageInstanceDto(IPageType pageType) : base(pageType.Id)
        {
            this.pageType = pageType.Id;
            this.name = this.pageType.Substring(this.pageType.LastIndexOf('.')+1);
            this.url = "";
        }
    }
}
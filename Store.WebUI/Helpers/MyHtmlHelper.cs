using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.WebUI.Models;
using System.Text;
using System.Web;

namespace Store.WebUI.Helpers
{
    public static class MyHtmlHelper
    {
        public static IHtmlContent PageLinks(this IHtmlHelper helper, PagingInfo pagingInfo, int pageCount, Func<int, string> pageUrl)
        {
            
            var pageNumbers = PagingRange.BuildPageRange(1, pagingInfo.TotalPages, pageCount);
            TagBuilder nav = new TagBuilder("nav");
            TagBuilder ul = new TagBuilder("ul"); // <ul></ul>
             // <nav> </nav>
            ul.AddCssClass("pagination justify-content-center");
            nav.MergeAttribute("aria-label", "Page navigation");

            //first page logic
            //{
            TagBuilder firstPage = new TagBuilder("li"); 
            firstPage.AddCssClass("page-item"); 
            if (pagingInfo.CurrentPage == 1)
            {
                firstPage.AddCssClass("disabled");
                
            }
            TagBuilder a_firstPage = new TagBuilder("a");
            a_firstPage.AddCssClass("page-link");
            a_firstPage.Attributes.Add("href", pageUrl(pagingInfo.CurrentPage - 1));
            a_firstPage.InnerHtml.Append("<");
            firstPage.InnerHtml.AppendHtml(a_firstPage);
            ul.InnerHtml.AppendHtml(firstPage);
            //}
            foreach (int i in pageNumbers)
            {
                TagBuilder li = new TagBuilder("li");

                TagBuilder a = new TagBuilder("a");
                li.AddCssClass("page-item");
                if (i == pagingInfo.CurrentPage) 
                {
                    li.AddCssClass("active");
                }
                a.Attributes.Add("href", pageUrl(i)); // a href=customer?page=1, href=products?page=2
                a.AddCssClass("page-link");
                a.InnerHtml.Append(i.ToString());
                //closingn
                li.InnerHtml.AppendHtml(a);

                ul.InnerHtml.AppendHtml(li);
                
            }

            //last page logic
            //{
            TagBuilder lastPage = new TagBuilder("li");
            lastPage.AddCssClass("page-item");
            if (pagingInfo.CurrentPage == pagingInfo.TotalPages)
            {
                lastPage.AddCssClass("disabled");

            }
            TagBuilder a_lastPage = new TagBuilder("a");
            a_lastPage.AddCssClass("page-link");
            a_lastPage.Attributes.Add("href", pageUrl(pagingInfo.CurrentPage - 1));
            a_lastPage.InnerHtml.Append(">");
            lastPage.InnerHtml.AppendHtml(a_lastPage);
            ul.InnerHtml.AppendHtml(lastPage);
            //}

            nav.InnerHtml.AppendHtml(ul);
            return new HtmlContentBuilder().AppendHtml(nav);
        }
    }
}

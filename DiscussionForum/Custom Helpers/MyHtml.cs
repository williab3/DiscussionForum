using DiscussionForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DiscussionForum.Custom_Helpers
{
    public static class MyHtml
    {
        public static IHtmlString Button(string text)
        {
            return new HtmlString(String.Format("<button class='btn btn-primary'>{0}</button>", text));
        }

        public static IHtmlString Button(string text, Object attributes)
        {
            Type paramType = attributes.GetType();
            StringBuilder buttonString = new StringBuilder("<button  ");
            StringBuilder classString = new StringBuilder("class='btn btn-primary ");
            HtmlString buttonHtml;
            foreach (PropertyInfo property in paramType.GetProperties())
            {
                string propertyName = property.Name;
                string propertyValue = property.GetValue(attributes).ToString();
                if (propertyName != "class")
                {
                    buttonString.Append(string.Format("{0}= {1} ", propertyName, propertyValue));
                }
                else
                {
                    classString.Append(propertyValue);
                }
            }
            classString.Append("'");
            buttonString.Append(classString);
            buttonString.AppendFormat(">{0}</button>", text);
            buttonHtml = new HtmlString(buttonString.ToString());
            return buttonHtml;
        }

        public static IHtmlString Button(string text, ButtonType buttonType)
        {
            return new HtmlString(String.Format("<button class='btn {1}'>{0}</button>", text, getButtonType(buttonType)));
        }

        public static IHtmlString Button(string text, ButtonType buttonType, string action, string controller)
        {
            return new HtmlString(String.Format("<a href='/{3}/{2}' class='btn {1}'>{0}</a>", text, getButtonType(buttonType), action, controller));
        }

        public static IHtmlString Button(string text, ButtonType buttonType, Object attributes)
        {
            Type paramType = attributes.GetType();
            StringBuilder buttonString = new StringBuilder("<button  ");
            foreach (PropertyInfo property in paramType.GetProperties())
            {
                string properName = property.Name;
                string propertyValue = property.GetValue(attributes).ToString();
                if (properName != "class")
                {
                    buttonString.Append(string.Format("{0}='{1}' ", properName, propertyValue));
                }
            }
            buttonString.Append(string.Format("class=' btn {0} '", getButtonType(buttonType)));
            buttonString.Append(string.Format(">{0}</button>", text));
            return new HtmlString(buttonString.ToString());
        }

        private static string getButtonType(ButtonType button)
        {
            switch (button)
            {
                case ButtonType.@default:
                    return "btn-default ";
                case ButtonType.primary:
                    return "btn-primary ";
                case ButtonType.success:
                    return "btn-success ";
                case ButtonType.info:
                    return "btn-info ";
                case ButtonType.warning:
                    return "btn-warning ";
                case ButtonType.danger:
                    return "btn-danger ";
                case ButtonType.link:
                    return "btn-link ";
                default:
                    return "btn ";
            }
        }

        public static MvcHtmlString File(this HtmlHelper html, string name)
        {
            var tb = new TagBuilder("input");
            tb.Attributes.Add("type", "file");
            tb.Attributes.Add("name", name);
            tb.GenerateId(name);
            return MvcHtmlString.Create(tb.ToString(TagRenderMode.SelfClosing));
        }

        public static string NullableConvertDate(DateTime? _date)
        {
            DateTime nonNullDate = _date ?? DateTime.MinValue;
            return nonNullDate.ToString("dd-MMM-yyyy");
        }

    }


    public class MyAjax
    {

    }

    public enum ButtonType
    {
        @default,
        primary,
        success,
        info,
        warning,
        danger,
        link,
    }

    public static class FeedWriter
    {
        public static HtmlString WriteComments(List<Comment> comments)
        {
            StringBuilder rawHtml = new StringBuilder();
            TagBuilder commentHtml;
            foreach (Comment comment in comments)
            {
                commentHtml = new TagBuilder("div");
                commentHtml.AddCssClass("comment");
                commentHtml.MergeAttribute("data-comment-id", comment.Id.ToString());

                TagBuilder header = new TagBuilder("div");
                header.AddCssClass("comment-header text-primary");
                header.InnerHtml += comment.Who;

                TagBuilder barSpan = new TagBuilder("span");
                barSpan.AddCssClass("gold-bar");
                barSpan.InnerHtml = "|";
                header.InnerHtml += barSpan;

                TagBuilder lastUpdatedSpan = new TagBuilder("span");
                lastUpdatedSpan.InnerHtml = MyHtml.NullableConvertDate(comment.LastUpdated);
                header.InnerHtml += lastUpdatedSpan;
                commentHtml.InnerHtml += header;

                TagBuilder body = new TagBuilder("div");
                body.AddCssClass("comment-body");
                body.InnerHtml += comment.What;
                commentHtml.InnerHtml += body;

                TagBuilder controls = new TagBuilder("div");
                controls.AddCssClass("comment-controls");

                TagBuilder thumbsUp = new TagBuilder("span");
                thumbsUp.AddCssClass("glyphicon glyphicon-thumbs-up voteIcon");
                controls.InnerHtml += thumbsUp;
                controls.InnerHtml += barSpan;

                TagBuilder thumbsDown = new TagBuilder("span");
                thumbsDown.AddCssClass("glyphicon glyphicon-thumbs-down voteIcon");
                controls.InnerHtml += thumbsDown;

                TagBuilder replyToggle = new TagBuilder("span");
                replyToggle.AddCssClass("voteIcon gold-bar reply-toggle");
                replyToggle.InnerHtml += "Reply";
                controls.InnerHtml += replyToggle;
                commentHtml.InnerHtml += controls;

                TagBuilder replyBlock = new TagBuilder("div");
                replyBlock.MergeAttribute("id", "reply" + comment.Id);
                replyBlock.AddCssClass("input-group");
                replyBlock.MergeAttribute("style", "display:none");

                TagBuilder txtBxReply = new TagBuilder("input");
                txtBxReply.AddCssClass("form-control txtBx-comment");
                txtBxReply.MergeAttribute("type", "text");
                txtBxReply.MergeAttribute("placeholder", "What do you think about this comment?");
                replyBlock.InnerHtml += txtBxReply;

                TagBuilder btnReply = new TagBuilder("span");
                btnReply.AddCssClass("input-group-addon btn btn-primary");
                btnReply.MergeAttribute("onclick", "postReply(this)");
                btnReply.InnerHtml += "Reply";
                replyBlock.InnerHtml += btnReply;
                commentHtml.InnerHtml += replyBlock;
                int nestCount = 0;
                if (comment.Replies.Count > 0)
                {
                    nestCount++;
                    writeReplies(comment.Replies, commentHtml, comment.Who, nestCount);
                }
            }
            
            return new HtmlString(rawHtml.ToString());
        }

        private static void writeReplies(List<Comment> _comments, TagBuilder htmlString,string personRepliedTo , int _nestCount)
        {
            foreach (Comment reply in _comments)
            {
                TagBuilder replyHtml = new TagBuilder("div");
                replyHtml.MergeAttribute("data-comment-id", reply.Id.ToString());
                replyHtml.AddCssClass("reply");

                TagBuilder header = new TagBuilder("div");
                header.AddCssClass("comment-header text-primary");
                header.InnerHtml += reply.Who;

                TagBuilder rightHand = new TagBuilder("span");
                rightHand.AddCssClass("glyphicon glyphicon-hand-right rightHand");
                header.InnerHtml += rightHand;
                header.InnerHtml += personRepliedTo;

                TagBuilder barspan = new TagBuilder("span");
                barspan.AddCssClass("gold-bar");
                barspan.InnerHtml += "|";
                header.InnerHtml += barspan;

                TagBuilder whenSpan = new TagBuilder("span");
                whenSpan.InnerHtml += MyHtml.NullableConvertDate(reply.LastUpdated);
                header.InnerHtml += whenSpan;
                replyHtml.InnerHtml += header;

                TagBuilder body = new TagBuilder("div");
                body.AddCssClass("comment-body");
                body.InnerHtml += reply.What;
                replyHtml.InnerHtml += body;

                TagBuilder controls = new TagBuilder("div");
                controls.AddCssClass("comment-controls");

                TagBuilder thumbsUp = new TagBuilder("span");
                thumbsUp.AddCssClass("glyphicon glyphicon-thumbs-up voteIcon");
                controls.InnerHtml += thumbsUp;
                controls.InnerHtml += barspan;

                TagBuilder thumbsDown = new TagBuilder("span");
                thumbsDown.AddCssClass("glyphicon glyphicon-thumbs-down voteIcon");
                controls.InnerHtml += thumbsDown;

                TagBuilder replyToggle = new TagBuilder("span");
                replyToggle.AddCssClass("voteIcon gold-bar reply-toggle");
                replyToggle.InnerHtml += "Reply";
                controls.InnerHtml += replyToggle;
                replyHtml.InnerHtml += controls;

                TagBuilder replyBlock = new TagBuilder("div");
                replyBlock.MergeAttribute("id", "reply" + reply.Id);
                replyBlock.AddCssClass("input-group");
                replyBlock.MergeAttribute("style", "display:none");

                TagBuilder txtBxReply = new TagBuilder("input");
                txtBxReply.AddCssClass("form-control txtBx-comment");
                txtBxReply.MergeAttribute("type", "text");
                txtBxReply.MergeAttribute("placeholder", "What do you think about this comment?");
                replyBlock.InnerHtml += txtBxReply;

                TagBuilder btnReply = new TagBuilder("span");
                btnReply.AddCssClass("input-group-addon btn btn-primary");
                btnReply.MergeAttribute("onclick", "postReply(this)");
                btnReply.InnerHtml += "Reply";
                replyBlock.InnerHtml += btnReply;
                replyHtml.InnerHtml += replyBlock;

                if (reply.Replies.Count > 0 && _nestCount < 5)
                {
                    _nestCount++;
                    writeReplies(reply.Replies, htmlString, reply.Who, _nestCount);
                }
                else if (reply.Replies.Count > 0)
                {

                }
            }
        }
    }
}


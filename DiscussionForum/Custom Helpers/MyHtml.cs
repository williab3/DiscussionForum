using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace DiscussionForum.Custom_Helpers
{
    public class MyHtml
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

        private static string getButtonType(ButtonType button)
        {
            switch (button)
            {
                case ButtonType.@default:
                    return "btn-default";
                case ButtonType.primary:
                    return "btn-primary";
                case ButtonType.success:
                    return "btn-success";
                case ButtonType.info:
                    return "btn-info";
                case ButtonType.warning:
                    return "btn-warning";
                case ButtonType.danger:
                    return "btn-danger";
                case ButtonType.link:
                    return "btn-link";
                default:
                    return "btn";
            }
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
}


﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace PeaLearning.Common.Utils
{
    public class StringUtils
    {
        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string QuoteString(string inputString)
        {
            string str = inputString.Trim();
            if (str != "")
            {
                str = str.Replace("'", "''");
            }
            return str;
        }

        public static string AddSlash(string input)
        {
            string str = !string.IsNullOrEmpty(input) ? input.Trim() : "";
            if (str != "")
            {
                str = str.Replace("'", "'").Replace("\"", "\\\"");
            }
            return str;
        }

        public static string RefreshText(string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text.Trim())) return text;

            text = HttpUtility.HtmlDecode(text);

            text = HttpUtility.UrlDecode(text);

            return text;
        }

        public static string RemoveStrHtmlTags(object inputObject)
        {
            if (inputObject == null)
            {
                return string.Empty;
            }
            string input = Convert.ToString(inputObject).Trim();
            if (input != "")
            {
                input = Regex.Replace(input, @"<(.|\n)*?>", string.Empty);
            }
            return input;
        }

        public static string ReplaceSpaceToPlus(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return Regex.Replace(input, @"\s+", "+", RegexOptions.IgnoreCase);
            }
            return input;
        }

        public static string ReplaceSpecialCharater(object inputObject)
        {
            if (inputObject == null)
            {
                return string.Empty;
            }
            return Convert.ToString(inputObject).Trim().Trim().Replace(@"\", @"\\").Replace("\"", "&quot;").Replace("“", "&ldquo;").Replace("”", "&rdquo;").Replace("‘", "&lsquo;").Replace("’", "&rsquo;").Replace("'", "&#39;");
        }

        public static string JavaScriptSring(string input)
        {
            input = input.Replace("'", @"\u0027");
            input = input.Replace("\"", @"\u0022");
            return input;
        }

        public static int CountWords(string stringInput)
        {
            if (string.IsNullOrEmpty(stringInput))
            {
                return 0;
            }
            stringInput = RemoveStrHtmlTags(stringInput);
            return Regex.Matches(stringInput, @"[\S]+").Count;
        }

        public static string GetEnumDescription(Enum value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(
                        typeof(DescriptionAttribute),
                        false);

                if (attributes != null &&
                    attributes.Length > 0)
                    return attributes[0].Description;
                else
                    return value.ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

     

        public static MemberInfo GetPropertyInformation(Expression propertyExpression)
        {
            //Debug.Assert(propertyExpression != null, "propertyExpression != null");
            MemberExpression memberExpr = propertyExpression as MemberExpression;
            if (memberExpr == null)
            {
                UnaryExpression unaryExpr = propertyExpression as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpr = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
            {
                return memberExpr.Member;
            }

            return null;
        }

        public static string SubWordInString(object obj, int maxWord, bool removeHTML = false)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            if (removeHTML) obj = RemoveStrHtmlTags(obj);

            string input = Regex.Replace(Convert.ToString(obj), @"\s+", " ");


            string[] strArray = Regex.Split(input, " ");
            if (strArray.Length <= maxWord)
            {
                return input;
            }
            input = string.Empty;
            for (int i = 0; i < maxWord; i++)
            {
                input = input + strArray[i] + " ";
            }
            return string.Concat(input.Trim(), "...");
        }

        public static string SubWordInDotString(object obj, int maxWord, string extensionEnd = " ...")
        {
            if (obj == null)
            {
                return string.Empty;
            }
            string input = Regex.Replace(Convert.ToString(obj), @"\s+", " ");
            string[] strArray = Regex.Split(input, " ");
            if (strArray.Length <= maxWord)
            {
                return input;
            }
            input = string.Empty;
            for (int i = 0; i < maxWord; i++)
            {
                input = input + strArray[i] + " ";
            }
            return (input.Trim() + extensionEnd);
        }

        public static string StripHtml(string html)
        {
            return (string.IsNullOrEmpty(html) ? string.Empty : Regex.Replace(html, "<.*?>", string.Empty));
        }

        public static string TrimText(object strIn, int intLength)
        {
            try
            {
                string str = StripHtml(Convert.ToString(strIn));
                if (str.Length > intLength)
                {
                    str = str.Substring(0, intLength - 4);
                    return (str.Substring(0, str.LastIndexOfAny(new char[] { ' ', '.', '?', ',', '!' })) + " ...");
                }
                return str;
            }
            catch (Exception)
            {
                return Convert.ToString(strIn);
            }
        }

        public static string FormatNumber(string sNumber, string sperator = ".")
        {
            int num = 3;
            int num2 = 0;
            for (int i = 1; i <= (sNumber.Length / 3); i++)
            {
                if ((num + num2) < sNumber.Length)
                {
                    sNumber = sNumber.Insert((sNumber.Length - num) - num2, sperator);
                }
                num += 3;
                num2++;
            }
            return sNumber;
        }

        public static string FormatNumberWithComma(string sNumber)
        {
            int num = 3;
            int num2 = 0;
            for (int i = 1; i <= (sNumber.Length / 3); i++)
            {
                if ((num + num2) < sNumber.Length)
                {
                    sNumber = sNumber.Insert((sNumber.Length - num) - num2, ",");
                }
                num += 3;
                num2++;
            }
            return sNumber;
        }

        public static bool IsValidWord(string input, char character)
        {
            if (string.IsNullOrEmpty(input))
            {
                return true;
            }
            string[] arr = input.Split(character);
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Length > 30)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static string GetMetaDescription(string format, params object[] args)
        {
            if (String.IsNullOrEmpty(format)) return String.Empty;

            string strDes = format;

            if (!String.IsNullOrEmpty(format) && args != null && args.Length > 0)
            {
                strDes = String.Format(strDes, args);
            }

            return strDes;
        }

        public static string ConvertNumberToCurrency(double number, string sperator = ".", string currentcy = "")
        {
            if (number <= 0)
            {
                return "0";
            }

            number = Math.Round(number, 0);

            string output = StringUtils.FormatNumber(number.ToString(CultureInfo.CurrentCulture), sperator) + currentcy;

            return output;
        }

        public static string ReplaceCaseInsensitive(string input, string[] search, string[] replacement)
        {
            int lenSearch = search.Length, lenRepalace = replacement.Length;
            string result = string.Empty;
            for (int i = 0; i < lenSearch; i++)
            {
                for (int j = 0; j < lenRepalace; j++)
                {
                    result = Regex.Replace(
                        input,
                        Regex.Escape(search[i]),
                        replacement[j].Replace("$", "$$"),
                        RegexOptions.IgnoreCase
                    ).Trim();
                    input = result;
                }
            }

            return result;
        }

        public static string GetStringTreeview(int level)
        {
            if (level == 0) return string.Empty;

            string strLevel = "";
            for (int i = 0; i < level; i++)
            {
                strLevel = strLevel + "__ ";
            }
            return strLevel;
        }


        public static string AddAttributeForAnchors(string htmlContent, string domainTarget = "http://banxehoi.com/diendan/seolink/?refer=", bool isEncrypt = false)
        {
            if (string.IsNullOrEmpty(htmlContent)) return htmlContent;
            try
            {
                htmlContent = Regex.Replace(htmlContent, @"rel=[""']nofollow[""']", string.Empty);
                htmlContent = htmlContent.Replace(@"target=[""']_blank[""']", string.Empty);

                string strRegex = @"(?<LINK><a[^>]href=[""'](?<url>[^""']+)[""'](?<attrs>[^>]*)>(?<Content>((?!<\/a>).)*)<\/a>)";
                Regex myRegex = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match match = myRegex.Match(htmlContent);
                //string strReplace = @"<a href=""" + domainTarget + @"${url}"" ${attrs} rel=""nofollow"" target=""_blank"">${Content}</a>";

                if (match.Success)
                {
                    htmlContent = myRegex.Replace(htmlContent, delegate (Match m)
                    {
                        string url = domainTarget + m.Groups["url"].Value;
                        string attrs = m.Groups["attrs"].Value;
                        string content = m.Groups["Content"].Value;
                        string link = string.Format(@"<a href=""{0}"" {1} rel=""nofollow"" target=""_blank"">{2}</a>", url, attrs,
                            content);
                        link = Regex.Replace(link, @"\s+", " ");
                        return link;
                    });
                    //htmlContent = myRegex.Replace(htmlContent, strReplace);
                }

            }
            catch
            {
                // Todo something
            }
            return htmlContent;
        }

        #region Unicode Process

        public const string uniChars =
            "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ";

        public const string unsignChar =
            "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU";

        public static string UnicodeToUnsignChar(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            string retVal = String.Empty;
            int pos;
            for (int i = 0; i < s.Length; i++)
            {
                pos = uniChars.IndexOf(s[i].ToString());
                if (pos >= 0)
                    retVal += unsignChar[pos];
                else
                    retVal += s[i];
            }
            return retVal;
        }

        public static string UnicodeToUnsignCharAndDash(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            const string strChar = "abcdefghijklmnopqrstxyzuvxw0123456789 -";
            //string retVal = UnicodeToKoDau(s);
            s = UnicodeToUnsignChar(s.ToLower().Trim());
            string sReturn = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (strChar.IndexOf(s[i]) > -1)
                {
                    if (s[i] != ' ')
                        sReturn += s[i];
                    else if (i > 0 && s[i - 1] != ' ' && s[i - 1] != '-')
                        sReturn += "-";
                }
            }
            while (sReturn.IndexOf("--") != -1)
            {
                sReturn = sReturn.Replace("--", "-");
            }
            return sReturn;
        }
        public static string RemoveSpecial(string s)
        {
            //const string REGEX = @"([^\w\dàáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ\.,\-_ ]+)";
            //s = Regex.Replace(s, REGEX, string.Empty, RegexOptions.IgnoreCase);

            return Regex.Replace(s, "[`~!@#$%^&*()_|+-=?;:'\"<>{}[]\\/]", string.Empty); //edited by vinhph

        }
        public static string RemoveSpecial4ModelDetail(string s)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(s))
            {
                result = Regex.Replace(s, "[+*%/^&:]", string.Empty, RegexOptions.IgnoreCase);
            }
            return result;
        }
        public static string ReplaceSpecial4ModelDetail(string s)
        {
            string result = string.Empty;
            result = Regex.Replace(s, "plus", "+", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "star", "*", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, "per", "%", RegexOptions.IgnoreCase);
            return result;
        }
        public static string UnicodeToKoDauAndSpace(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            string retVal = String.Empty;
            int pos;
            for (int i = 0; i < s.Length; i++)
            {
                pos = uniChars.IndexOf(s[i].ToString());
                if (pos >= 0)
                    retVal += unsignChar[pos];
                else
                    retVal += s[i];
            }
            return retVal;
        }
        /// <summary>
        /// loại bỏ các ký tự không phải chữ, số, dấu cách thành ký tự không dấu
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharToKhongDau(string s)
        {
            string retVal = UnicodeToUnsignChar(s);
            Regex regex = new Regex(@"[^\d\w]+");
            retVal = regex.Replace(retVal, " ");
            while (retVal.IndexOf("  ") != -1)
            {
                retVal = retVal.Replace("  ", " ");
            }
            return retVal;
        }

        #endregion
    }
}

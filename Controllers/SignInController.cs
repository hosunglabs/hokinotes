using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Script.Serialization;

namespace hosungnotes.Controllers
{
    public class SignInController : Controller
    {
        // GET: SignIn
        public ActionResult Index()
        {
            return View("LogIn");
        }

        public ActionResult LogOn()
        {
            if (Request.QueryString["ReturnValue"] != null)
            {
                ViewData["strError"] = SetErrorType(Int32.Parse(Request.QueryString["ReturnValue"]));
            }

            return View();
        }

        [HttpGet]
        public ActionResult Authenticate(string userId, string passwd)
        {
            // 잘못된요청
            if (!String.IsNullOrWhiteSpace(userId) && !String.IsNullOrWhiteSpace(passwd))
            {
                String uId = System.Configuration.ConfigurationManager.AppSettings["UserId"];
                String uPwd = System.Configuration.ConfigurationManager.AppSettings["PassWord"];

                if (userId.ToLower() == uId.ToLower())
                {
                    // 로그인 성공
                    if (passwd == uPwd)
                    {
                        GWPrincipalSerializeModel serializeModel = new GWPrincipalSerializeModel();
                        serializeModel.UserId = uId;

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        string userData = serializer.Serialize(serializeModel);

                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, serializeModel.UserSeq.ToString(), DateTime.Now, DateTime.Now.AddDays(1), false, userData);

                        string encTicket = FormsAuthentication.Encrypt(authTicket);

                        HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                        faCookie.Expires = System.DateTime.Now.AddHours(1);
                        Response.Cookies.Add(faCookie);

                        return RedirectToAction("Index", "Search");
                    }
                    else
                        return RedirectToAction("LogOn", "SignIn", new { ReturnValue = LoginErrorCode.FAIL_LOGIN });
                }
                else
                    return RedirectToAction("LogOn", "SignIn", new { ReturnValue = LoginErrorCode.NOTHING_MEMBER });

            }
            else
                return RedirectToAction("LogOn", "SignIn", new { ReturnValue = LoginErrorCode.DENY_SERVICE });
        }

        public ActionResult LogOff()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                authCookie.Expires = DateTime.Now.AddDays(-1);
                authCookie = null;
            }

            return RedirectToAction("LogOn", "SignIn", new { ReturnValue = LoginErrorCode.EXPIRE_COOKIE });
        }

        public string SetErrorType(int nReturnValue)
        {
            string strError = string.Empty;

            switch (nReturnValue)
            {
                case 0: break;
                case LoginErrorCode.FAIL_LOGIN:
                    strError = "<span class='point_red'>로그인에 실패</span>했습니다.<br /><strong>아이디(ID)</strong>와 <strong>비밀번호</strong>를 <strong>확인</strong>하고 다시 로그인해주세요."; break;
                case LoginErrorCode.NO_AUTHORITY_ACCESS:
                    strError = "<span class='point_red'>로그인에 실패</span>했습니다.<br />접근 가능한 페이지가 없습니다."; break;
                case LoginErrorCode.EXPIRE_COOKIE:
                    strError = "로그아웃되었습니다."; break;
                case LoginErrorCode.TIME_OUT:
                    strError = "<span class='point_red'>시간이 만료되었습니다. 다시 시도 해 주세요.</span>"; break;
                case LoginErrorCode.DENY_SERVICE:
                    strError = "<span class='point_red'>잘못된 요청입니다. 다시 시도 해 주세요.</span>"; break;
                case LoginErrorCode.NOTHING_MEMBER:
                    strError = "<span class='point_red'>등록되지 않은 사용자 입니다.</span>"; break;
                case LoginErrorCode.SERVER_ERROR:
                    strError = "<span class='point_red'>서버 오류.</span>"; break;
                default:
                    strError = "<span class='point_red'>내부 오류</span>"; break;
            }

            return strError;
        }
    }

    public class LoginErrorCode
    {

        #region 로그인 에러코드

        public const int NO_ERROR = 0;

        /// <summary>
        /// 비밀번호 틀림
        /// </summary>
        public const int FAIL_LOGIN = -1;

        /// <summary>
        /// 검증 실패
        /// </summary>
        public const int FAIL_VERIFY = -2;

        /// <summary>
        /// 시간 만료
        /// </summary>
        public const int TIME_OUT = -3;

        /// <summary>
        /// 페이지 접근 권한 없음
        /// </summary>
        public const int NO_AUTHORITY_ACCESS = -4;

        /// <summary>
        /// 쿠키 만료
        /// </summary>
        public const int EXPIRE_COOKIE = -5;

        /// <summary>
        /// 요청 거부
        /// </summary>
        public const int DENY_SERVICE = -6;

        /// <summary>
        /// 서버 오류
        /// </summary>
        public const int SERVER_ERROR = -7;

        /// <summary>
        /// 사용자 없음
        /// </summary>
        public const int NOTHING_MEMBER = -8;
        #endregion

    }
}
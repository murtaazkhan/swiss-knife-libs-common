using SwissKnife.Libs.Common.Audit.Models;
using SwissKnife.Libs.Common.Constants;
using SwissKnife.Libs.Common.Helpers;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace SwissKnife.Libs.Common.Audit.Middlewares;

/// <summary>
/// Http Request Auditing Middleware
/// </summary>
public class HttpRequestAuditMiddleware
    {
        /// <summary>
        /// Http Request Delegate
        /// </summary>
        private readonly RequestDelegate _next;


        /// <summary>
        /// Constructor that take the next RequestDelegate in the chain as a parameter
        /// </summary>
        /// /// <param name="next">The next RequestDelegate in the chain</param>
        public HttpRequestAuditMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// This method constructs the http request audit object. 
        /// It also adds the 'Request-Id' header to the the response.
        /// </summary>
        /// /// <param name="context">Context containing response where the Header will be placed</param>      
        public async Task Invoke(HttpContext context)
        {
            var requestAudit = GetHttpRequestAuditModel(context);

            context.Response.OnStarting(delegate
            {
                var traceId = Activity.Current?.TraceId.ToString();
                if (!string.IsNullOrEmpty(traceId))
                {
                    context.Response.Headers.Add(HttpHeaders.REQUEST_ID, traceId);
                }

                return Task.CompletedTask;
            });

            context.Items.Add(HttpHeaders.HTTP_REQUEST_AUDIT, requestAudit);

            await _next(context);
        }

        #region Private Methods
        /// <summary>
        /// This method constructs the Http Request Audit model
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/></param>
        /// <returns><see cref="HttpRequestAuditModel"/></returns>
        private HttpRequestAuditModel GetHttpRequestAuditModel(HttpContext context)
        {
            var authorizationToken = context?.Request?.Headers[HttpHeaders.AUTHORIZATION].ToString();
            
            var result = new HttpRequestAuditModel()
            {
                Timestamp = DateTime.UtcNow,
                ClientIpAddress = context?.Connection?.RemoteIpAddress.ToString(),
                ClientSource = context?.Request?.Headers[HttpHeaders.USER_AGENT].ToString(),
                MethodType = context?.Request?.Method.ToString(),
                RequestUrl = context?.Request?.Host.ToString() + context?.Request?.Path.ToString(),
                Status = context?.Response?.StatusCode.ToString(),
                Authorization = new AuthorizationModel()
                {
                    Name = JwtHelper.GetClaimValueFromToken(authorizationToken, JwtTokenClaims.NAME),
                    Roles = JwtHelper.GetClaimValueFromToken(authorizationToken, JwtTokenClaims.ROLES),
                    IpAddress = JwtHelper.GetClaimValueFromToken(authorizationToken, JwtTokenClaims.IP_ADDRESS),
                    TenantId = JwtHelper.GetClaimValueFromToken(authorizationToken, JwtTokenClaims.TENANT_ID),
                    Issuer = JwtHelper.GetClaimValueFromToken(authorizationToken, JwtTokenClaims.ISSUER),
                    AppId = JwtHelper.GetClaimValueFromToken(authorizationToken, JwtTokenClaims.APP_ID),
                    Audience = JwtHelper.GetClaimValueFromToken(authorizationToken, JwtTokenClaims.AUDIENCE),
                    Scope = JwtHelper.GetClaimValueFromToken(authorizationToken, JwtTokenClaims.SCOPE),
                    Username = JwtHelper.GetClaimValueFromToken(authorizationToken,JwtTokenClaims.UPN) 
                            ?? JwtHelper.GetClaimValueFromToken(authorizationToken, JwtTokenClaims.UNIQUE_NAME)
                            ?? JwtHelper.GetClaimValueFromToken(authorizationToken, JwtTokenClaims.EMAIL)
                }
            };

            return result;
        }
        #endregion
    }


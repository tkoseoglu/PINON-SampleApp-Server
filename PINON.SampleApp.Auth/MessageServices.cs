using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TOLGA.Common.Contracts;
using TOLGA.Common.Models;

namespace PINON.SampleApp.Identity
{
    public class EmailService : IIdentityMessageService
    {        
        private readonly ITolgaLogging _tolgaLogging;
        private readonly ITolgaUtilities _tolgaUtilities;

        public EmailService(ITolgaLogging tolgaLogging, ITolgaUtilities tolgaUtilities)
        {
            this._tolgaLogging = tolgaLogging;
            this._tolgaUtilities = tolgaUtilities;            
        }

        public Task SendAsync(IdentityMessage message)
        {
            this._tolgaLogging.LogInfo($"SendAsync: destination, {message.Destination}, subject: {message.Subject}, body: {message.Body}");

            //this comes usually from config file or app settings db table
            var fromName = "Tolga Koseoglu";
            var fromEmail = "tolga.k@outlook.com";

            var toAddr = new List<MailAddress> { new MailAddress(message.Destination) };
            var email = new TolgaEmail
            {
                FromName = fromName == "" ? Common.Constants.SentEmailFromName : fromName,
                FromEmail = fromEmail == "" ? Common.Constants.SentEmailFromEmailAddress : fromEmail,
                Subject = message.Subject,
                Body = message.Body,
                ToAddresses = toAddr
            };

            using (var emailExt = new EmailExt(this._tolgaLogging, this._tolgaUtilities))
            {
                emailExt.SendEmails(email);
            }

            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}

using System;
using PINON.SampleApp.Common;
using TOLGA.Common.Contracts;
using TOLGA.Common.Models;
using TransactionResult = TOLGA.Common.Models.TransactionResult;

namespace PINON.SampleApp.Identity
{
    public class EmailExt : IDisposable
    {
        private readonly ITolgaLogging _tolgaLogging;
        private readonly ITolgaUtilities _tolgaUtilities;

        public EmailExt(ITolgaLogging tolgaLogging, ITolgaUtilities tolgaUtilities)
        {
            _tolgaLogging = tolgaLogging;
            _tolgaUtilities = tolgaUtilities;
        }

        public void Dispose()
        {
        }

        public TransactionResult SendEmails(TolgaEmail email)
        {
            var result = new TransactionResult();
            var emailConnection = new TolgaEmailConnection
            {
                Port = Constants.EmailHostPort,
                IsSsl = true,
                Host = Constants.EmailHost ?? "smtp.gmail.com",
                UserName = Constants.EmailUserName ?? "tolgaonline@gmail.com",
                Password = Constants.EmailPassword ?? "Ktk19766"
            };

            result = _tolgaUtilities.SendEmail(email, emailConnection);
            return result;
        }
    }
}
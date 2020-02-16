using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace TestNepal.Dtos
{
    public class Common
    {
        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
       
            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }
        public static object JsonOkObject(object obj)
        {
            var result = new
            {
                status = "ok",
                data = obj
            };
            return result;
        }
        public static object JsonErrorObject(string Errormsg)
        {
            var result = new
            {
                status = "error",
                data = Errormsg
            };
            return result;
        }
        public static string GetSequence(string code, Int64 Id)
        {
            string seq = DateTime.Now.ToString("yy") + "" + Id.ToString("000000000");
            if (code == "customer")
                return "A" + seq;
            if (code == "customercontact")
                return "C" + seq;
            if (code == "customertask")
                return "T" + seq;
            if (code == "customerappointment")
            {
                seq = DateTime.Now.ToString("yy") + "" + Id.ToString("00000000");
                return "AP" + seq;
            }
            if (code == "customeropportunity")
                return "O" + seq;
            else if (code == "customersite")
                return "S" + seq;
            else if (code == "building")
                return "B" + seq;
            else if (code == "supplier")
            {
                seq = DateTime.Now.ToString("yy") + "" + Id.ToString("0000000");
                return "Sup" + seq;
            }
            else if (code == "quote")
                return "Q" + seq;
            else if (code == "Job")
                return "J" + seq;
            return "";
        }
    }

    public enum JobStatus
    {
        DRAFT,
       UNPAID,
        PAID,
        PUBLIC, // After limit hours
        PUBLIC2, // After 4 hours of job
        ACCEPTED,
        CANCELED,
        COMPLETED,
        PAYMENTERROR
    }

    public enum JobEventType
    {
        UPCOMMING,
        PAST,
        CANCELED,
        POOL,
        MYJOBS
    }
}
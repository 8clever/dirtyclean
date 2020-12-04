using System;
using System.Collections.Generic;
using DeltaDNA;

namespace UnityEngine.GameGrowth
{
    public class TransactionData
    {
        public const string amazonPurchaseTokenParamName = "amazonPurchaseToken";
        public const string amazonUserIdParamName = "amazonUserID";
        public const string engagementIdParamName = "engagementID";
        public const string isInitiatorParamName = "isInitiator";
        public const string paymentCountryParamName = "paymentCountry";
        public const string productIdParamName = "productID";
        public const string revenueValidatedParamName = "revenueValidated";
        public const string sdkVersionParamName = "sdkVersion";
        public const string transactionIdParamName = "transactionID";
        public const string transactionReceiptParamName = "transactionReceipt";
        public const string transactionReceiptSignatureParamName = "transactionReceiptSignature";
        public const string transactionServerParamName = "transactionServer";
        public const string transactorIdParamName = "transactorID";
        public const string userLevelParamName = "userLevel";
        public const string userScoreParamName = "userScore";
        public const string userXpParamName = "userXP";

        public string transactionName { get; set; }
        public TransactionType transactionType { get; set; }
        public Product productsReceived { get; set; }
        public Product productsSpent { get; set; }
        public double localizedPrice { get; set; }
        public string isoCurrencyCode { get; set; }
        public string amazonPurchaseToken { get; set; }
        public string amazonUserId { get; set; }
        public int? engagementId { get; set; }
        public bool? isInitiator { get; set; }
        public PaymentCountry? paymentCountry { get; set; }
        public string productId { get; set; }
        public int? revenueValidated { get; set; }
        public string sdkVersion { get; set; }
        public string transactionId { get; set; }
        public string transactionReceipt { get; set; }
        public string transactionReceiptSignature { get; set; }
        public TransactionServer? transactionServer { get; set; }
        public string transactorId { get; set; }
        public int? userLevel { get; set; }
        public int? userScore { get; set; }
        public int? userXp { get; set; }
        
        public TransactionData(string transactionName, 
            TransactionType transactionType, 
            Product productsReceived, 
            Product productsSpent, 
            double localizedPrice, 
            string isoCurrencyCode
            )
        {
            this.transactionName = transactionName;
            this.transactionType = transactionType;
            this.productsReceived = productsReceived;
            this.productsSpent = productsSpent;
            this.localizedPrice = localizedPrice;
            this.isoCurrencyCode = isoCurrencyCode;
        }
    }
    
    public enum TransactionType
    {
        Sale,
        Purchase,
        Trade,
    }

    public static class TransactionTypeExtensions
    {
        static Dictionary<TransactionType, string> s_EventExportValues;
        
        static TransactionTypeExtensions()
        {
            s_EventExportValues = new Dictionary<TransactionType, string>();
            var values = Enum.GetValues(typeof(TransactionType));
            foreach (var value in values)
            {
                var transactionType = (TransactionType)value;
                s_EventExportValues.Add(transactionType, transactionType.ToString().ToUpper());
            }
        }
        
        public static string ExportForEvent(this TransactionType transactionType)
        {
            return s_EventExportValues[transactionType];
        }
    }

    public enum TransactionServer
    {
        Apple,
        Amazon,
        Google,
        Valve,
    }
    
    public static class TransactionServerExtensions
    {
        static Dictionary<TransactionServer, string> s_EventExportValues;
        
        static TransactionServerExtensions()
        {
            s_EventExportValues = new Dictionary<TransactionServer, string>();
            var values = Enum.GetValues(typeof(TransactionServer));
            foreach (var value in values)
            {
                var transactionServer = (TransactionServer)value;
                s_EventExportValues.Add(transactionServer, transactionServer.ToString().ToUpper());
            }
        }
        
        public static string ExportForEvent(this TransactionServer transactionServer)
        {
            return s_EventExportValues[transactionServer];
        }
    }

    public enum PaymentCountry
    {
     A1,
     A2,
     AD,
     AE,
     AF,
     AG,
     AI,
     AL,
     AM,
     AO,
     AQ,
     AR,
     AS,
     AT,
     AU,
     AW,
     AX,
     AZ,
     BA,
     BB,
     BD,
     BE,
     BF,
     BG,
     BH,
     BI,
     BJ,
     BL,
     BM,
     BN,
     BO,
     BQ,
     BR,
     BS,
     BT,
     BV,
     BW,
     BY,
     BZ,
     CA,
     CC,
     CD,
     CF,
     CG,
     CH,
     CI,
     CK,
     CL,
     CM,
     CN,
     CO,
     CR,
     CU,
     CV,
     CW,
     CX,
     CY,
     CZ,
     DE,
     DJ,
     DK,
     DM,
     DO,
     DZ,
     EC,
     EE,
     EG,
     EH,
     ER,
     ES,
     ET,
     EU,
     FI,
     FJ,
     FK,
     FM,
     FO,
     FR,
     GA,
     GB,
     GD,
     GE,
     GF,
     GG,
     GH,
     GI,
     GL,
     GM,
     GN,
     GP,
     GQ,
     GR,
     GS,
     GT,
     GU,
     GW,
     GY,
     HK,
     HM,
     HN,
     HR,
     HT,
     HU,
     ID,
     IE,
     IL,
     IM,
     IN,
     IO,
     IR,
     IQ,
     //I  R, //NOTE: This is a wrong country code from DDNA website
     IS,
     IT,
     JE,
     JM,
     JO,
     JP,
     KE,
     KG,
     KH,
     KI,
     KM,
     KN,
     KP,
     KR,
     KW,
     KY,
     KZ,
     LA,
     LB,
     LC,
     LI,
     LK,
     LR,
     LS,
     LT,
     LU,
     LV,
     LY,
     MA,
     MC,
     MD,
     ME,
     MF,
     MG,
     MH,
     MK,
     ML,
     MM,
     MN,
     MO,
     MP,
     MQ,
     MR,
     MS,
     MT,
     MU,
     MV,
     MW,
     MX,
     MY,
     MZ,
     NA,
     NC,
     NE,
     NF,
     NG,
     NI,
     NL,
     NO,
     NP,
     NR,
     NU,
     NZ,
     O1,
     OM,
     PA,
     PE,
     PF,
     PG,
     PH,
     PK,
     PL,
     PM,
     PN,
     PR,
     PS,
     PT,
     PW,
     PY,
     QA,
     RE,
     RO,
     RS,
     RU,
     RW,
     SA,
     SB,
     SC,
     SD,
     SE,
     SG,
     SH,
     SI,
     SJ,
     SK,
     SL,
     SM,
     SN,
     SO,
     SR,
     SS,
     ST,
     SV,
     SX,
     SY,
     SZ,
     TC,
     TD,
     TF,
     TG,
     TH,
     TJ,
     TK,
     TL,
     TM,
     TN,
     TO,
     TR,
     TT,
     TV,
     TW,
     TZ,
     UA,
     UG,
     UM,
     US,
     UY,
     UZ,
     VA,
     VC,
     VE,
     VG,
     VI,
     VN,
     VU,
     WF,
     WS,
     YE,
     YT,
     ZA,
     ZM,
     ZW,
    }
}
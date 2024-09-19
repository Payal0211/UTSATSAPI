namespace UTSATSAPI.Helpers.Common
{
    public static class AppSettingProvider
    {
        /// <summary>
        /// Gets or sets the name of the SMTP email.
        /// </summary>
        /// <value>
        /// The name of the SMTP email.
        /// </value>
        public static string SMTPEmailName { get; set; }

        /// <summary>
        /// Gets or sets the name of the SMTP password.
        /// </summary>
        /// <value>
        /// The name of the SMTP password.
        /// </value>
        public static string SMTPPasswordName { get; set; }

        /// <summary>
        /// Gets or sets the name of the SMTP client.
        /// </summary>
        /// <value>
        /// The name of the SMTP client.
        /// </value>
        public static string SMTPClientName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [SMTPSSL name].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [SMTPSSL name]; otherwise, <c>false</c>.
        /// </value>
        public static bool SMTPSSLName { get; set; }

        /// <summary>
        /// Gets or sets the name of the SMTP port.
        /// </summary>
        /// <value>
        /// The name of the SMTP port.
        /// </value>
        public static int SMTPPortName { get; set; }

    }
}

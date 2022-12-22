namespace Mephi.Cyber.AuthService.Core.Models
{
    
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.yale.edu/tp/cas")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.yale.edu/tp/cas", IsNullable = false)]
    public partial class serviceResponse
    {

        private serviceResponseAuthenticationSuccess authenticationSuccessField;

        /// <remarks/>
        public serviceResponseAuthenticationSuccess authenticationSuccess
        {
            get
            {
                return this.authenticationSuccessField;
            }
            set
            {
                this.authenticationSuccessField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.yale.edu/tp/cas")]
    public partial class serviceResponseAuthenticationSuccess
    {

        private string userField;

        private serviceResponseAuthenticationSuccessAttributes attributesField;

        /// <remarks/>
        public string user
        {
            get
            {
                return this.userField;
            }
            set
            {
                this.userField = value;
            }
        }

        /// <remarks/>
        public serviceResponseAuthenticationSuccessAttributes attributes
        {
            get
            {
                return this.attributesField;
            }
            set
            {
                this.attributesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.yale.edu/tp/cas")]
    public partial class serviceResponseAuthenticationSuccessAttributes
    {

        private System.DateTime authenticationDateField;

        private bool longTermAuthenticationRequestTokenUsedField;

        private bool isFromNewLoginField;

        private string fioField;

        private string fullnameField;

        /// <remarks/>
        public System.DateTime authenticationDate
        {
            get
            {
                return this.authenticationDateField;
            }
            set
            {
                this.authenticationDateField = value;
            }
        }

        /// <remarks/>
        public bool longTermAuthenticationRequestTokenUsed
        {
            get
            {
                return this.longTermAuthenticationRequestTokenUsedField;
            }
            set
            {
                this.longTermAuthenticationRequestTokenUsedField = value;
            }
        }

        /// <remarks/>
        public bool isFromNewLogin
        {
            get
            {
                return this.isFromNewLoginField;
            }
            set
            {
                this.isFromNewLoginField = value;
            }
        }

        /// <remarks/>
        public string fio
        {
            get
            {
                return this.fioField;
            }
            set
            {
                this.fioField = value;
            }
        }

        /// <remarks/>
        public string fullname
        {
            get
            {
                return this.fullnameField;
            }
            set
            {
                this.fullnameField = value;
            }
        }
    }
}
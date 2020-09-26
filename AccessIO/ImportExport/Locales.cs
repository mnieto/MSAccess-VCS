using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using dao = Microsoft.Office.Interop.Access.Dao;

namespace AccessIO {
//interesting access page
//http://allenbrowne.com/tips.html#Prog Solution
//Set or create a property
//http://visualbasic.about.com/od/usevb6/l/aa101602a.htm
//Build table
//http://allenbrowne.com/func-DAO.html#MakeGuidTable
//http://allenbrowne.com/func-ADOX.html#CreateDatabaseAdox

    class Locales {
        Dictionary<string, string> locales;

        public Locales() {
            locales = new Dictionary<string, string>();
            LoadLocaleStrings();
        }

        private void LoadLocaleStrings() {

            FieldInfo[] fields = typeof(dao.LanguageConstants).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields) {
                string code = LangCode(field.GetValue(null).ToString());
                if (!locales.ContainsKey(code))
                    locales.Add(code, field.Name);
            }
            locales.Remove(dao.LanguageConstants.dbLangNordic);  //Only for Jet 1.0
        }

        private string LangCode(string langDescription) {
            string code = langDescription.Substring(11);
            int pos = code.IndexOf(';');
            code = code.Substring(0, pos);
            return Convert.ToInt32(code, 16).ToString();
        }

        public string this[string localeCode] {
            get {
                return locales[localeCode];
            }
        }


        
    }
}

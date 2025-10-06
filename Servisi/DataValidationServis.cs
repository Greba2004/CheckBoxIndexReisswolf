using System;
using System.Globalization;
using System.Windows.Forms;

namespace CheckBoXIndexAPP.Servisi
{
    public class DataValidationService
    {
        /// <summary>
        /// Validira obavezna polja (samo 3 glavna + check naziv).
        /// </summary>
        public bool ValidirajObaveznaPolja(string[] polja, string[] naziviPolja, bool[] poljaObavezna)
        {
            // Proveravamo samo prva 4 polja
            int maxIndex = Math.Min(4, polja.Length);

            for (int i = 0; i < maxIndex; i++)
            {
                if (i >= poljaObavezna.Length || i >= naziviPolja.Length)
                    continue;

                if (poljaObavezna[i] && string.IsNullOrWhiteSpace(polja[i]))
                {
                    MessageBox.Show($"Polje '{naziviPolja[i]}' je obavezno i ne može biti prazno!");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validira datume OD i DO (ostaje isto).
        /// </summary>
        public bool ValidirajDatume(string datumOdText, string datumDoText)
        {
            if (!string.IsNullOrWhiteSpace(datumOdText))
            {
                if (!DateTime.TryParseExact(datumOdText, "dd.MM.yyyy.", null, DateTimeStyles.None, out _))
                {
                    MessageBox.Show("Datum OD nije validan. Koristite format: dd.MM.yyyy.");
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(datumDoText))
            {
                if (!DateTime.TryParseExact(datumDoText, "dd.MM.yyyy.", null, DateTimeStyles.None, out _))
                {
                    MessageBox.Show("Datum DO nije validan. Koristite format: dd.MM.yyyy.");
                    return false;
                }
            }

            return true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulkaisukanavatietokannanSynkkaus
{
    class Apufunktiot
    {

        // nama stop wordsit hypataan yli eli naita ei oteta huomioon pitkissa stringeissa (julkaisun nimi, kustantaja jne.)
        private string[] stop_words = {" i "," me "," my "," myself "," we "," our "," ours "," ourselves "," you "," your "," yours "," yourself "," yourselves "," he "," him "," his "," himself "," she "," her "," hers "," herself "," it "," its "," itself "," they "," them "," their "," theirs ",
" themselves "," what "," which "," who "," whom "," this "," that "," these "," those "," am "," is "," are "," was "," were "," be "," been "," being "," have "," has "," had "," having "," do "," does "," did "," doing "," a "," an "," the "," and "," but "," if "," or "," because "," as "," until ",
" while "," of "," at "," by "," for "," with "," about "," against "," between "," into "," through "," during "," before "," after "," above "," below "," to "," from "," up "," down "," in "," out "," on "," off "," over "," under "," again "," further "," then "," once "," here "," there ",
" when "," where "," why "," how "," all "," any "," both "," each "," few "," more "," most "," other "," some "," such "," no "," nor "," not "," only "," own "," same "," so "," than "," too "," very "," s "," t "," can "," will "," just "," don "," should "," now "};

        // nama stop charsit hypataan yli eli naita ei oteta mukaan pitkissa stringeissa (julkaisun nimi)
        private string[] stop_chars_name = { "!", "\"", "#", "$", "%", "&", "'", "(", ")", "*", "+", ",", "-", ".", "/", ":", ";", "<", "=", ">", "?", "@", "[", "\\", "]", "^", "_", "`", "{", "|", "}", "~", "£", "¿", 
                                        "®", "¬", "½", "¼", "«", "»", "©", "┐", "└", "┴", "┬", "├", "─", "┼", "┘", "┌", "¦", "¯", "´", "≡", "±", "‗", "¾", "¶", "§", "÷", "¸", "°", "¨", "·", "¹", "³", "²" };

        // nama stop charsit hypataan yli eli naita ei oteta mukaan pitkissa stringeissa (other_title).
        // HUOM! tama eroaa ylla olevasta vain silla, etta tassa ei ole mukana merkkia ";". Sita ei ole koska silla erotetaan other_titlet ja sita tarvitaan myohemmin.
        private string[] stop_chars_other_title = { "!", "\"", "#", "$", "%", "&", "'", "(", ")", "*", "+", ",", "-", ".", "/", ":", "<", "=", ">", "?", "@", "[", "\\", "]", "^", "_", "`", "{", "|", "}", "~", "£", "¿", 
                                        "®", "¬", "½", "¼", "«", "»", "©", "┐", "└", "┴", "┬", "├", "─", "┼", "┘", "┌", "¦", "¯", "´", "≡", "±", "‗", "¾", "¶", "§", "÷", "¸", "°", "¨", "·", "¹", "³", "²" };


        Tietokantaoperaatiot tietokantaoperaatiot = new Tietokantaoperaatiot();

        // Muokataan parametrina annettua nimea siten, etta nimesta poistetaan stop wordsit ja stop charsit.
        // Lisaksi alusta poistetaan the, a ja an -merkit ja merkkijono trimmataan.
        // Palautetaan muokattu merkkijono.
        public string muokkaa_nimea(string nimi, string name_or_other_title)
        {

            string[] stop_chars = stop_chars_name;  // alustetaan stop_chars_name:ksi

            // Muutetaan nimi LowerCase:ksi ja trimmataan
            nimi = nimi.ToLower().Trim();

            // tutkitaan tarkastellaanko name- vai other_title -kenttaa
            if (name_or_other_title.Equals("other_title"))
            {
                stop_chars = stop_chars_other_title;

            }
   

            // Kaydaan lapi stop_chars -merkit ja poistetaan merkki mikali se loytyy nimesta
            foreach (string c in stop_chars)
            {
                if (nimi.Contains(c))
                {
                    nimi = nimi.Replace(c, " ");
                }
            }

            // Trimmataan taas nimi
            nimi = nimi.Trim();

            // Kaydaan lapi stop_words -sanat ja poistetaan sana mikali se loytyy nimesta
            foreach (string item in stop_words)
            {
                if (nimi.Contains(item))
                {
                    nimi = nimi.Replace(item, " ");
                }
            }

            // poistetaan tyhjat valimerkit
            nimi = nimi.Replace("     ", " ");
            nimi = nimi.Replace("    ", " ");
            nimi = nimi.Replace("   ", " ");
            nimi = nimi.Replace("  ", " ");

            // Jalleen trimmataan nimi
            nimi.Trim();

            // Poistetaan sitten nimen alusta sanat the, a ja an
            string sana = "";

            for (int i = 0; i < nimi.Length; i++)
            {

                if (nimi[i] != ' ')
                {
                    sana = sana + nimi[i];
                }

                else
                {
                    sana = sana + nimi[i];

                    if (sana.Equals("the ") || sana.Equals("a ") || sana.Equals("an "))
                    {
                        nimi = nimi.Replace(sana, "").Trim();
                    }

                    break;
                }


            }

            return nimi;

        }

    }

}

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;


namespace JulkaisukanavatietokannanSynkkaus
{
    class Program
    {
        static void Main(string[] args)
        {

            //////////////////////////////////////////////////////////////////////////////////////////
            // Ohjelmalle annetaan kolme argumenttia, jotka ovat seuraavat:
            // 1. args[0] = tietokantapalvelimen nimi (dwidvirta / dwitsuorasql / dwipjutisql1)
            // 2. args[1] = polku Extract-kansioon, jonne zip-file puretaan ja josta zip-file alussa tuhotaan
            // 3. args[2] = polku, jonne massa.zip-tiedosto asetetaan
            ////////////////////////////////////////////////////////////////////////////////////////////

            Tietokantaoperaatiot tietokantaoperaatiot = new Tietokantaoperaatiot();
            TiedostoOperaatiot tiedostoOperaatiot = new TiedostoOperaatiot();
            Apufunktiot apufunktiot = new Apufunktiot();

            if (args.Length != 3)
            {
                Console.Write("argumenttien maara on vaara");
            }

            else
            {
                string server = args[0];
                string pathToExtractedFiles = args[1];
                string zipPath = args[2];

                string jufo_History_Default = "2012:;2013:;2014:;2015:;2016:;2017:;2018:;2019:;2020:;2021:;";     // tama on default-arvo, jos rajapinnasta ei loydy Jufo_History-arvoa

                tiedostoOperaatiot.tuhoaExtractKansio(pathToExtractedFiles);        // tuhotaan extract-kansio
                tiedostoOperaatiot.haeZipRajapinnasta(zipPath);                     // haetaan zip-kansio rajapinnasta
                tiedostoOperaatiot.puraZipTiedosto(zipPath, pathToExtractedFiles);  // puretaan zip-kansio

                // Parsitaan sitten json-tiedosto
                JArray stuff = JArray.Parse(File.ReadAllText(@pathToExtractedFiles + "\\var\\www\\files\\v1.1\\massa.json"));

                int amountOfObjects = stuff.Count();

                for (int i = 0; i < amountOfObjects; i++)
                {

                    // Tutkitaan loytyyko rajapinnassa oleva Jufo_ID jo SQL-serverin julkaisukanavatietokannassa 

                    // Haetaan ensin kanavan tiedot rajapinnasta 
                    string jufo_ID_API = stuff[i]["Jufo_ID"].ToString();
                    string channel_ID_API = stuff[i]["Channel_ID"].ToString();

                    // jos apin palauttama jufo_ID on tyhja tai null, tarkistetaan onko channel_ID eri kuin tyhjä tai null
                    // mikali kummatkin ovat null tai tyhja, niin jatketaan seuraavalta kierrokselta
                    if ((jufo_ID_API == null) || jufo_ID_API.Equals(""))
                    {
                        if ((channel_ID_API == null) || channel_ID_API.Equals(""))
                        {
                            continue;  
                        }  
                    }

                    // tehdaan tarkistus myos toisinpain. Jos APIn palauttamat channel_ID on tyhja tai null, tarkistetaan onko jufo_ID tyhja tai null
                    // mikali kummatkin ovat null tai tyhja, niin jatketaan seuraavalta kierrokselta
                    if ((channel_ID_API == null) || channel_ID_API.Equals(""))
                    {
                        if ((jufo_ID_API == null) || jufo_ID_API.Equals(""))
                        {
                            continue;
                        }
                    }


                    string jufo_Luokka_API = stuff[i]["Level"].ToString();
                    string name_API = stuff[i]["Name"].ToString();
                    string other_Title_API = stuff[i]["Other_Title"].ToString();
                    string publisher_API = stuff[i]["Publisher"].ToString();

                    // Muokataan name_APIn ja other_Title_APIn nimea mikali niissa on stop wordseja tai stop charseja
                    if ((name_API != null) && !(name_API.Equals("")))
                    {
                        name_API = apufunktiot.muokkaa_nimea(name_API, "name");
                    }

                    if ((other_Title_API != null) && !(other_Title_API.Equals("")))
                    {
                        other_Title_API = apufunktiot.muokkaa_nimea(other_Title_API, "other_title");
                    }

                    if ((publisher_API != null) && !(publisher_API.Equals("")))
                    {
                        publisher_API = apufunktiot.muokkaa_nimea(publisher_API, "publisher");
                    }

                    string type_API = stuff[i]["Type"].ToString();
                    string ISSNL_API = stuff[i]["ISSNL"].ToString();
                    string ISSN1_API = stuff[i]["ISSN1"].ToString();
                    string ISSN2_API = stuff[i]["ISSN2"].ToString();
                    string ISBN_API = stuff[i]["ISBN"].ToString();
                    string active_API = stuff[i]["Active"].ToString();
                    string jufo_History_API = stuff[i]["Jufo_history"].ToString();
                    int active_binary_API = Int32.Parse(stuff[i]["Active_Binary"].ToString());

                    // jos jufo_History_API == "", niin asetetaan sille merkkijono
                    if (jufo_History_API.Equals(""))
                    {
                        jufo_History_API = jufo_History_Default;
                    }


                    string year_End_API = stuff[i]["Year_End"].ToString();

                    // Tarkistetaan onko year_End_API = "". Jos on, niin se pitaa muuttaa arvoon 9999
                    int year_End_API_after_check = 9999;

                    if (!year_End_API.Equals(""))
                    {
                        year_End_API_after_check = Int32.Parse(year_End_API);
                    }

                    // tutkitaan loytyyko jufo_ID_APIa tai channel_ID_APIa vastaava julkaisukanava SQL Serverin julkaisukanavatietokannasta
                    bool channelIsFoundFromVirta = tietokantaoperaatiot.kanavaLoytyyVirrasta(server, jufo_ID_API, channel_ID_API);


                    // Jos kanava ei loydy SQL-server -kannasta, niin lisataan uusi rivi kantaan
                    if (!channelIsFoundFromVirta)
                    {
                        // Lisataan uusi rivi julkaisut_mds.dbo.Julkaisukanavatietokanta -tauluun
                        tietokantaoperaatiot.insert_Julkaisukanavatietokanta(server, jufo_ID_API, channel_ID_API, jufo_Luokka_API, name_API, other_Title_API, publisher_API, type_API, ISSNL_API, ISSN1_API, ISSN2_API, ISBN_API, active_API, jufo_History_API, year_End_API_after_check, active_binary_API);
                    }


                    // Jos mennaan tahan else-haaraan, niin kanava loytyy SQL Server -kannasta.         

                    // Jos kanava loytyy SQL-server -kannasta, niin tarkistetaan loytyyko eroavaisuuksia
                    // rajapinnan palauttaman (=MySQL-kanta) ja SQL Server -kannan valilla.            
                    // Jos eroavaisuuksia loytyy, niin paivitetaan SQL Server -kantaan se arvo,       
                    // joka loytyy rajapinnasta.                                                      
                    else
                    {

                        // Haetaan SQL Server -kannasta Jufo_ID_APIa tai Channel_ID_APIa vastaavan kanavan tiedot
                        string connectionString = "Server=" + server + ";Database=julkaisut_mds;Trusted_Connection=true";
                        SqlConnection conn = new SqlConnection(connectionString);
                        SqlDataReader reader = tietokantaoperaatiot.hae_julkaisukanavatietokannasta(conn, jufo_ID_API, channel_ID_API);

                        while (reader.Read())
                        {

                            int id = (int) reader["ID"];
                            string jufo_ID = reader["Jufo_ID"] == System.DBNull.Value ? null : (string) reader["Jufo_ID"];
                            string channel_ID = reader["Channel_ID"] == System.DBNull.Value ? null : (string) reader["Channel_ID"];
                            string jufo_Luokka = reader["Jufo_Luokka"] == System.DBNull.Value ? null : (string)reader["Jufo_Luokka"];
                            string name = reader["Name"] == System.DBNull.Value ? null : (string)reader["Name"];
                            string other_Title = reader["Other_Title"] == System.DBNull.Value ? null : (string)reader["Other_Title"];
                            string publisher = reader["Publisher"] == System.DBNull.Value ? null : (string)reader["Publisher"];
                            string type = reader["Type"] == System.DBNull.Value ? null : (string)reader["Type"];
                            string ISSNL = reader["ISSNL"] == System.DBNull.Value ? null : (string)reader["ISSNL"];
                            string ISSN1 = reader["ISSN1"] == System.DBNull.Value ? null : (string)reader["ISSN1"];
                            string ISSN2 = reader["ISSN2"] == System.DBNull.Value ? null : (string)reader["ISSN2"];
                            string ISBN = reader["ISBN"] == System.DBNull.Value ? null : (string)reader["ISBN"];
                            string active = reader["Active"] == System.DBNull.Value ? null : (string)reader["Active"];
                            string jufo_history = reader["Jufo_History"] == System.DBNull.Value ? null : (string)reader["Jufo_History"];
                            int year_end = (int)reader["Year_End"];
                            int active_binary = (int)reader["Active_binary"];


                            // Verrataan sitten rajapinnasta loytyvia arvoja SQL Server:in arvoihin.
                            // Jos loytyy eroavaisuuksia, niin paivitetaan erot SQL Server -kantaan

                            // case Jufo_Luokka
                            if ((jufo_Luokka == null) && (!jufo_Luokka_API.Equals("")))
                            {
                                // earlier it was like this:
                                //tietokantaoperaatiot.update_julkaisukanavatietokanta_jufo_Luokka(server, jufo_ID_API, jufo_Luokka_API);
                                // now we use id instead of jufo_ID
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_jufo_Luokka(server, id, jufo_Luokka_API);
                            }
                            else if ((jufo_Luokka != null) && (!jufo_Luokka.Equals(jufo_Luokka_API)))
                            {
                                //tietokantaoperaatiot.update_julkaisukanavatietokanta_jufo_Luokka(server, jufo_ID_API, jufo_Luokka_API);
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_jufo_Luokka(server, id, jufo_Luokka_API);
                            }


                            // case Name
                            if ((name == null) && (!name_API.Equals("")))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_name(server, id, name_API);
                            }
                            else if ((name != null) && (!name.Equals(name_API)))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_name(server, id, name_API);
                            }


                            // case Other_Title
                            if ((other_Title == null) && (!other_Title_API.Equals("")))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_other_Title(server, id, other_Title_API);
                            }
                            else if ((other_Title != null) && (!other_Title.Equals(other_Title_API)))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_other_Title(server, id, other_Title_API);
                            }

                            // case Publisher
                            if ((publisher == null) && (!publisher_API.Equals("")))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_publisher(server, id, publisher_API);
                            }
                            else if ((publisher != null) && (!publisher.Equals(publisher_API)))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_publisher(server, id, publisher_API);
                            }



                            // case Type
                            if ((type == null) && (!type_API.Equals("")))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_type(server, id, type_API);
                            }
                            else if ((type != null) && (!type.Equals(type_API)))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_type(server, id, type_API);
                            }


                            // case ISSNL
                            if ((ISSNL == null) && (!ISSNL_API.Equals("")))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_ISSNL(server, id, ISSNL_API);
                            }
                            else if ((ISSNL != null) && (!ISSNL.Equals(ISSNL_API)))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_ISSNL(server, id, ISSNL_API);
                            }


                            // case ISSN1
                            if ((ISSN1 == null) && (!ISSN1_API.Equals("")))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_ISSN1(server, id, ISSN1_API);
                            }
                            else if ((ISSN1 != null) && (!ISSN1.Equals(ISSN1_API)))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_ISSN1(server, id, ISSN1_API);
                            }


                            // case ISSN2
                            if ((ISSN2 == null) && (!ISSN2_API.Equals("")))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_ISSN2(server, id, ISSN2_API);
                            }
                            else if ((ISSN2 != null) && (!ISSN2.Equals(ISSN2_API)))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_ISSN2(server, id, ISSN2_API);
                            }


                            // case ISBN
                            if ((ISBN == null) && (!ISBN_API.Equals("")))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_ISBN(server, id, ISBN_API);
                            }
                            else if ((ISBN != null) && (!ISBN.Equals(ISBN_API)))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_ISBN(server, id, ISBN_API);
                            }


                            // case Active
                            if ((active == null) && (!active_API.Equals("")))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_active(server, id, active_API);
                            }
                            else if ((active != null) && (!active.Equals(active_API)))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_active(server, id, active_API);
                            }


                            // Case Jufo_History
                            if (jufo_history == null)
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_jufo_history(server, id, jufo_History_API);
                            }
                            else if (!jufo_history.Equals(jufo_History_API))
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_jufo_history(server, id, jufo_History_API);
                            }


                            // case Year_End
                            if (year_end != year_End_API_after_check)
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_year_end(server, id, year_End_API_after_check);
                            }


                            // case Active_binary
                            if (active_binary != active_binary_API)
                            {
                                tietokantaoperaatiot.update_julkaisukanavatietokanta_active_binary(server, id, active_binary_API);
                            }

                        }

                        reader.Close();
                        conn.Close();



                    }

                }

            }

            //Console.ReadLine();
            Environment.Exit(0);

        }

    }

}


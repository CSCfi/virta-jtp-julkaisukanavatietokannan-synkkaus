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
//using Newtonsoft.Json.Linq;

namespace JulkaisukanavatietokannanSynkkaus
{
    class TiedostoOperaatiot
    {


        // Tuhotaan kansio, jossa on purettu zip-tiedosto. Zip-tiedosto sisaltaa json-tiedoston
        public void tuhoaExtractKansio(string path)
        {
            if (Directory.Exists(@path))
            {
                Directory.Delete(@path, true);
            }
        }


        // Haetaan zip-tiedosto rajapinnasta 
        public void haeZipRajapinnasta(string zipPath)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebClient client = new WebClient();

            string url = "https://jufo-rest.csc.fi/v1.1/massa.json.zip";

            client.DownloadFile(url, @zipPath);
        }



        // Puretaan zip-tiedosto
        public void puraZipTiedosto(string zipPath, string pathToExtractedFiles)
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(@zipPath, @pathToExtractedFiles);
        }

    }

}

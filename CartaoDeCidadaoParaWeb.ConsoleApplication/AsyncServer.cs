using CSJ2K.Util;
using eidpt;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace CartaoDeCidadaoParaWeb.ConsoleApplication
{
    /// <summary>
    /// 
    /// </summary>
    public class AsyncServer
    {
        /// <summary>
        /// The reader
        /// </summary>
        private string reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncServer"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public AsyncServer(string reader)
        {
            this.reader = reader;

            var listener = new HttpListener();

            listener.Prefixes.Add("http://localhost:11111/");
            listener.Prefixes.Add("http://127.0.0.1:11111/");

            listener.Start();

            while (true)
            {
                try
                {
                    var context = listener.GetContext();
                    ThreadPool.QueueUserWorkItem(o => HandleRequest(context));
                }
                catch (Exception)
                {
                    // TODO: Tratar excepção.
                }
            }
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="state">The state.</param>
        private void HandleRequest(object state)
        {
            var context = (HttpListenerContext)state;
            try
            {
                context.Response.StatusCode = 200;
                context.Response.SendChunked = true;

                var callback =
                    context.Request.QueryString["callback"];

                Cidadao cidadao = null;

                try
                {
                    cidadao = this.ObterDadosCartaoCidadao(this.reader);
                }
                catch (Exception)
                {
                    throw;
                }

                var x = new string[] { "value1", "value2" };

                var serialized =
                    JsonConvert.SerializeObject(cidadao);

                var callbackRetorno = callback + "('" + serialized + "')";


                var bytes = Encoding.UTF8.GetBytes(callbackRetorno + "\n");
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);

            }
            catch (Exception ex)
            {
                // TODO: Tratar excepção.
            }
            finally
            {
                context.Response.OutputStream.Close();
            }
        }

        /// <summary>
        /// Obters the dados cartao cidadao.
        /// </summary>
        /// <param name="leitor">The leitor.</param>
        /// <returns></returns>
        private Cidadao ObterDadosCartaoCidadao(string leitor)
        {
            Cidadao cidadao = null;

            try
            {
                Pteid.Init(leitor);
                Pteid.SetSODChecking(0);

                var x =
                    Pteid.GetID();

                var pins =
                   Pteid.GetPINs();

                var pinMorada =
                    pins.Where(fi => fi.id == 131).FirstOrDefault();

                PteidAddr address = null;

                // Descomentar linhas abaixo para ler a morada.
                // Nota: Vai pedir o PIN de morada, geralmente é: 0000

                //if (pinMorada.triesLeft > 0)
                //{
                //    address =
                //        Pteid.GetAddr();
                //}

                // Obter a fotografia e converter para byte[].

                var y = Pteid.GetPic();

                byte[] photo = null;

                using (MemoryStream ms = new MemoryStream(y.picture, 0, y.piclength, false))
                {

                    BitmapImageCreator.Register();

                    var tempImage = CSJ2K.J2kImage.FromStream(ms);
                    ms.Close();

                    var img = tempImage.As<Bitmap>();
                    using (MemoryStream ms2 = new MemoryStream())
                    {
                        img.Save(ms2, ImageFormat.Jpeg);

                        photo = ms2.ToArray();
                    }
                }

                cidadao = new Cidadao()
                {
                    NomeCompleto = ConverterString(x.firstname + " " + x.name),
                    NumBI = x.numBI,
                    NIF = x.numNIF,
                    SNS = x.numSNS,
                    SS = x.numSS,
                    Sexo = x.sex,
                    DataValidade = DateTime.Parse(x.validityDate),
                    DataNascimento = DateTime.Parse(x.birthDate),
                    Nacionalidade = ConverterString(x.nationality),
                    Fotografia = Convert.ToBase64String(photo),
                    EntidadeEmissora = ConverterString(x.deliveryEntity),
                    
                    //TODO: Adicionar outros campo
            };

                if (address != null)
                {
                    // TODO: Caso a prorpiedade "streettype" venha a vazio ler a rua no campo "place".

                    cidadao.Rua = ConverterString(address.streettype) + " " + ConverterString(address.street) + ", " + ConverterString(address.door);
                    cidadao.CodigoPostal = address.cp4 + "-" + address.cp3;
                    cidadao.Localidade = ConverterString(address.locality);
                    cidadao.Municipio = ConverterString(address.municipalityDesc);
                    cidadao.Distrito = ConverterString(address.districtDesc);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

                Pteid.Exit(0);
            }

            return cidadao;
        }

        /// <summary>
        /// Converters the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private string ConverterString(string value)
        {
            var bytes =
             System.Text.Encoding.Default.GetBytes(value);

            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}

using System;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace ProyectoVenta.NEGOCIO
{
    public class EmailService
    {
        private string smtpServer;
        private int smtpPort;
        private string emailFrom;
        private string emailPassword;
        private string emailDisplayName;

        public EmailService()
        {
            // Leer configuración desde Web.config
            smtpServer = ConfigurationManager.AppSettings["SmtpServer"] ?? "smtp.gmail.com";
            smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"] ?? "587");
            emailFrom = ConfigurationManager.AppSettings["EmailFrom"] ?? "";
            emailPassword = ConfigurationManager.AppSettings["EmailPassword"] ?? "";
            emailDisplayName = ConfigurationManager.AppSettings["EmailDisplayName"] ?? "Pizzería Bambino";
        }

        public bool EnviarReporte(string emailDestino, string asunto, string cuerpoHtml, byte[] pdfAttachment = null, string nombreArchivo = "reporte.pdf")
        {
            try
            {
                if (string.IsNullOrEmpty(emailFrom) || string.IsNullOrEmpty(emailPassword))
                {
                    throw new Exception("Las credenciales de email no están configuradas. Revise el archivo Web.config.");
                }

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom, emailDisplayName);
                    mail.To.Add(emailDestino);
                    mail.Subject = asunto;
                    mail.Body = cuerpoHtml;
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.Normal;

                    // Adjuntar PDF si existe
                    if (pdfAttachment != null && pdfAttachment.Length > 0)
                    {
                        var attachment = new Attachment(new System.IO.MemoryStream(pdfAttachment), nombreArchivo, "application/pdf");
                        mail.Attachments.Add(attachment);
                    }

                    using (SmtpClient smtp = new SmtpClient(smtpServer, smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(emailFrom, emailPassword);
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Timeout = 30000; // 30 segundos
                        smtp.Send(mail);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al enviar email: " + ex.Message);
            }
        }

        public string GenerarCuerpoReporte(DataTable ventas, DateTime fechaInicio, DateTime fechaFin, string nombreCliente = "Todos")
        {
            decimal total = 0;
            int totalVentas = ventas.Rows.Count;
            string filasHtml = "";

            foreach (DataRow row in ventas.Rows)
            {
                total += Convert.ToDecimal(row["total"]);
                filasHtml += $@"
                    <tr style='border-bottom: 1px solid #ddd;'>
                        <td style='padding: 12px 8px; text-align: left;'>{row["numero_venta"]}</td>
                        <td style='padding: 12px 8px; text-align: center;'>{Convert.ToDateTime(row["fecha_venta"]):dd/MM/yyyy HH:mm}</td>
                        <td style='padding: 12px 8px; text-align: left;'>{row["nombre_cliente"]}</td>
                        <td style='padding: 12px 8px; text-align: left;'>{row["telefono"]}</td>
                        <td style='padding: 12px 8px; text-align: right; font-weight: bold;'>Bs. {Convert.ToDecimal(row["total"]):N2}</td>
                        <td style='padding: 12px 8px; text-align: center;'>
                            <span style='background-color: {GetEstadoColor(row["estado_venta"].ToString())}; color: white; padding: 4px 8px; border-radius: 4px; font-size: 11px;'>
                                {row["estado_venta"]}
                            </span>
                        </td>
                        <td style='padding: 12px 8px; text-align: center;'>{row["metodo_pago"]}</td>
                    </tr>";
            }

            string html = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <style>
                        body {{
                            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                            margin: 0;
                            padding: 0;
                            background-color: #f5f5f5;
                        }}
                        .container {{
                            max-width: 900px;
                            margin: 20px auto;
                            background-color: white;
                            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
                        }}
                        .header {{
                            background: linear-gradient(135deg, #dc3545 0%, #c82333 100%);
                            color: white;
                            padding: 30px 20px;
                            text-align: center;
                        }}
                        .header h1 {{
                            margin: 0;
                            font-size: 28px;
                        }}
                        .header h2 {{
                            margin: 10px 0 0 0;
                            font-weight: 300;
                            font-size: 18px;
                        }}
                        .info-section {{
                            background-color: #f8f9fa;
                            padding: 20px;
                            border-left: 4px solid #28a745;
                        }}
                        .info-item {{
                            margin: 8px 0;
                            font-size: 14px;
                        }}
                        .info-item strong {{
                            color: #333;
                        }}
                        .content {{
                            padding: 20px;
                        }}
                        table {{
                            width: 100%;
                            border-collapse: collapse;
                            margin-top: 20px;
                            font-size: 13px;
                        }}
                        th {{
                            background-color: #28a745;
                            color: white;
                            padding: 12px 8px;
                            text-align: left;
                            font-weight: 600;
                            border: none;
                        }}
                        .total-section {{
                            background-color: #e9f7ef;
                            padding: 20px;
                            margin-top: 20px;
                            border-left: 4px solid #28a745;
                        }}
                        .total-section h3 {{
                            margin: 0;
                            color: #28a745;
                            font-size: 20px;
                        }}
                        .footer {{
                            background-color: #f8f9fa;
                            padding: 20px;
                            text-align: center;
                            color: #666;
                            font-size: 12px;
                            border-top: 1px solid #ddd;
                        }}
                        .stats {{
                            display: inline-block;
                            background-color: #17a2b8;
                            color: white;
                            padding: 10px 20px;
                            border-radius: 20px;
                            margin: 10px 5px;
                            font-size: 14px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>🍕 PIZZERÍA BAMBINO</h1>
                            <h2>Reporte de Ventas</h2>
                        </div>
                        
                        <div class='info-section'>
                            <div class='info-item'><strong>📅 Período:</strong> {fechaInicio:dd/MM/yyyy} al {fechaFin:dd/MM/yyyy}</div>
                            <div class='info-item'><strong>👤 Cliente:</strong> {nombreCliente}</div>
                            <div class='info-item'><strong>📊 Total de ventas:</strong> {totalVentas}</div>
                        </div>
                        
                        <div class='content'>
                            <table>
                                <thead>
                                    <tr>
                                        <th>Nro. Pedido</th>
                                        <th style='text-align: center;'>Fecha</th>
                                        <th>Cliente</th>
                                        <th>Teléfono</th>
                                        <th style='text-align: right;'>Total</th>
                                        <th style='text-align: center;'>Estado</th>
                                        <th style='text-align: center;'>Pago</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {filasHtml}
                                </tbody>
                            </table>
                            
                            <div class='total-section'>
                                <h3>💰 Total Acumulado: Bs. {total:N2}</h3>
                                <div style='margin-top: 10px;'>
                                    <span class='stats'>📦 {totalVentas} ventas</span>
                                    <span class='stats'>💵 Promedio: Bs. {(totalVentas > 0 ? total / totalVentas : 0):N2}</span>
                                </div>
                            </div>
                        </div>
                        
                        <div class='footer'>
                            <p>Reporte generado automáticamente el {DateTime.Now:dd/MM/yyyy} a las {DateTime.Now:HH:mm}</p>
                            <p>Pizzería Bambino - Sistema de Gestión de Ventas</p>
                        </div>
                    </div>
                </body>
                </html>";

            return html;
        }

        private string GetEstadoColor(string estado)
        {
            switch (estado.ToLower())
            {
                case "pendiente":
                    return "#ffc107";
                case "completado":
                case "entregado":
                    return "#28a745";
                case "en proceso":
                    return "#17a2b8";
                case "cancelado":
                    return "#dc3545";
                default:
                    return "#6c757d";
            }
        }
    }
}

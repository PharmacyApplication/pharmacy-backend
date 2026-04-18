using Microsoft.EntityFrameworkCore;
using PharmacyAPI.Data;
using PharmacyAPI.Helpers;
using PharmacyAPI.Models;
using PharmacyAPI.Repositories.Interfaces;
using PharmacyAPI.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace PharmacyAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailLogRepository _emailLogRepo;
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public EmailService(IEmailLogRepository emailLogRepo, AppDbContext context, IConfiguration config)
        {
            _emailLogRepo = emailLogRepo;
            _context = context;
            _config = config;
        }

        private SmtpClient CreateSmtpClient()
        {
            var host = _config["EmailSettings:SmtpHost"]!;
            var port = int.Parse(_config["EmailSettings:SmtpPort"]!);
            var senderEmail = _config["EmailSettings:SenderEmail"]!;
            var senderPassword = _config["EmailSettings:SenderPassword"]!;

            return new SmtpClient(host, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, senderPassword)
            };
        }

        private MailMessage CreateMailMessage(User user, string subject, string body)
        {
            var senderEmail = _config["EmailSettings:SenderEmail"]!;
            var senderName = _config["EmailSettings:SenderName"] ?? "PharmacyApp";

            var mail = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(new MailAddress(user.Email, user.FullName));
            return mail;
        }

        private async Task<Order> LoadFullOrderAsync(Order order)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Medicine)
                .FirstAsync(o => o.OrderId == order.OrderId);
        }

        public async Task SendOrderConfirmationAsync(User user, Order order)
        {
            const string emailType = "OrderConfirmation";
            string errorMessage = string.Empty;
            string status = "Sent";

            try
            {
                var fullOrder = await LoadFullOrderAsync(order);
                var body = EmailTemplateHelper.GenerateOrderConfirmation(user, fullOrder);
                var mail = CreateMailMessage(user, $"Order Confirmed – #{fullOrder.OrderId}", body);
                using var smtp = CreateSmtpClient();
                await smtp.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                status = "Failed";
                errorMessage = ex.Message;
            }
            finally
            {
                await _emailLogRepo.SaveLogAsync(new EmailLog
                {
                    UserId = user.UserId,
                    OrderId = order.OrderId,
                    EmailType = emailType,
                    SentAt = DateTime.UtcNow,
                    Status = status,
                    ErrorMessage = string.IsNullOrEmpty(errorMessage) ? null : errorMessage
                });
            }
        }

        public async Task SendShippedNotificationAsync(User user, Order order)
        {
            const string emailType = "Shipped";
            string errorMessage = string.Empty;
            string status = "Sent";

            try
            {
                var fullOrder = await LoadFullOrderAsync(order);
                var body = EmailTemplateHelper.GenerateShippedNotification(user, fullOrder);
                var mail = CreateMailMessage(user, $"Your Order #{fullOrder.OrderId} Has Been Shipped!", body);
                using var smtp = CreateSmtpClient();
                await smtp.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                status = "Failed";
                errorMessage = ex.Message;
            }
            finally
            {
                await _emailLogRepo.SaveLogAsync(new EmailLog
                {
                    UserId = user.UserId,
                    OrderId = order.OrderId,
                    EmailType = emailType,
                    SentAt = DateTime.UtcNow,
                    Status = status,
                    ErrorMessage = string.IsNullOrEmpty(errorMessage) ? null : errorMessage
                });
            }
        }

        public async Task SendDeliveredNotificationAsync(User user, Order order)
        {
            const string emailType = "Delivered";
            string errorMessage = string.Empty;
            string status = "Sent";

            try
            {
                var fullOrder = await LoadFullOrderAsync(order);
                var body = EmailTemplateHelper.GenerateDeliveredNotification(user, fullOrder);
                var mail = CreateMailMessage(user, $"Order #{fullOrder.OrderId} Delivered – Thank You!", body);
                using var smtp = CreateSmtpClient();
                await smtp.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                status = "Failed";
                errorMessage = ex.Message;
            }
            finally
            {
                await _emailLogRepo.SaveLogAsync(new EmailLog
                {
                    UserId = user.UserId,
                    OrderId = order.OrderId,
                    EmailType = emailType,
                    SentAt = DateTime.UtcNow,
                    Status = status,
                    ErrorMessage = string.IsNullOrEmpty(errorMessage) ? null : errorMessage
                });
            }
        }
    }
}

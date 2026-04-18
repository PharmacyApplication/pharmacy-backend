using PharmacyAPI.Models;

namespace PharmacyAPI.Helpers
{
    public static class EmailTemplateHelper
    {
        private static string BaseTemplate(string title, string accentColor, string bodyContent) => $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"" />
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0""/>
  <title>{title}</title>
  <style>
    body {{ margin:0; padding:0; background:#f4f6f9; font-family: 'Segoe UI', Arial, sans-serif; color:#333; }}
    .wrapper {{ max-width:620px; margin:40px auto; background:#fff; border-radius:10px; overflow:hidden; box-shadow:0 4px 20px rgba(0,0,0,0.08); }}
    .header {{ background:{accentColor}; padding:30px 40px; color:#fff; }}
    .header h1 {{ margin:0; font-size:22px; font-weight:700; letter-spacing:0.5px; }}
    .header p {{ margin:6px 0 0; font-size:14px; opacity:0.88; }}
    .body {{ padding:32px 40px; }}
    .body p {{ line-height:1.7; font-size:15px; margin:0 0 16px; }}
    table.items {{ width:100%; border-collapse:collapse; margin:20px 0; font-size:14px; }}
    table.items th {{ background:#f0f0f0; padding:10px 12px; text-align:left; font-weight:600; color:#555; }}
    table.items td {{ padding:10px 12px; border-bottom:1px solid #eee; }}
    .total-row td {{ font-weight:700; font-size:15px; color:#222; border-top:2px solid #ddd; border-bottom:none; }}
    .info-box {{ background:#f9f9f9; border-left:4px solid {accentColor}; padding:14px 18px; border-radius:4px; margin:20px 0; font-size:14px; }}
    .info-box strong {{ display:block; margin-bottom:4px; color:#444; }}
    .footer {{ background:#f4f6f9; padding:20px 40px; text-align:center; font-size:12px; color:#999; }}
    .badge {{ display:inline-block; padding:4px 12px; border-radius:20px; font-size:12px; font-weight:600; background:{accentColor}; color:#fff; }}
  </style>
</head>
<body>
  <div class=""wrapper"">
    <div class=""header"">
      <h1>PharmacyApp</h1>
      <p>{title}</p>
    </div>
    <div class=""body"">
      {bodyContent}
    </div>
    <div class=""footer"">
      &copy; {DateTime.UtcNow.Year} PharmacyApp. All rights reserved.<br/>
      This is an automated message — please do not reply.
    </div>
  </div>
</body>
</html>";

        private static string BuildItemsTable(Order order)
        {
            var rows = string.Concat(order.OrderItems.Select(item => $@"
              <tr>
                <td>{item.Medicine?.Name ?? "Medicine"}</td>
                <td style=""text-align:center"">{item.Quantity}</td>
                <td style=""text-align:right"">₹{item.UnitPrice:F2}</td>
                <td style=""text-align:right"">₹{(item.Quantity * item.UnitPrice):F2}</td>
              </tr>"));

            return $@"
            <table class=""items"">
              <thead>
                <tr>
                  <th>Medicine</th>
                  <th style=""text-align:center"">Qty</th>
                  <th style=""text-align:right"">Unit Price</th>
                  <th style=""text-align:right"">Subtotal</th>
                </tr>
              </thead>
              <tbody>
                {rows}
                <tr class=""total-row"">
                  <td colspan=""3"">Total</td>
                  <td style=""text-align:right"">₹{order.TotalAmount:F2}</td>
                </tr>
              </tbody>
            </table>";
        }

        public static string GenerateOrderConfirmation(User user, Order order)
        {
            var itemsTable = BuildItemsTable(order);
            var body = $@"
              <p>Hi <strong>{user.FullName}</strong>,</p>
              <p>Thank you for your order! We've received it and it's being processed. Here's a summary:</p>
              <div class=""info-box"">
                <strong>Order #</strong>{order.OrderId}
                <strong style=""margin-top:8px"">Status</strong><span class=""badge"">Ordered</span>
                <strong style=""margin-top:8px"">Placed At</strong>{order.PlacedAt:dd MMM yyyy, hh:mm tt} UTC
              </div>
              {itemsTable}
              <div class=""info-box"">
                <strong>Shipping Address</strong>
                {order.ShippingAddress}
              </div>
              <p>We'll notify you once your order is shipped. If you have any questions, contact our support team.</p>
              <p>Thank you for trusting PharmacyApp!</p>";

            return BaseTemplate("Order Confirmation", "#2563eb", body);
        }

        public static string GenerateShippedNotification(User user, Order order)
        {
            var body = $@"
              <p>Hi <strong>{user.FullName}</strong>,</p>
              <p>Great news — your order <strong>#{order.OrderId}</strong> has been shipped and is on its way to you!</p>
              <div class=""info-box"">
                <strong>Order #</strong>{order.OrderId}
                <strong style=""margin-top:8px"">Status</strong><span class=""badge"" style=""background:#f59e0b"">Shipped</span>
                <strong style=""margin-top:8px"">Shipped At</strong>{order.UpdatedAt:dd MMM yyyy, hh:mm tt} UTC
              </div>
              <p>Estimated delivery: <strong>2–5 business days</strong> depending on your location.</p>
              <div class=""info-box"">
                <strong>Delivery Address</strong>
                {order.ShippingAddress}
              </div>
              <p>Please ensure someone is available to receive the package at the above address.</p>
              <p>Thank you for shopping with PharmacyApp!</p>";

            return BaseTemplate("Your Order Has Been Shipped!", "#f59e0b", body);
        }

        public static string GenerateDeliveredNotification(User user, Order order)
        {
            var body = $@"
              <p>Hi <strong>{user.FullName}</strong>,</p>
              <p>Your order <strong>#{order.OrderId}</strong> has been successfully delivered. We hope you're happy with your purchase!</p>
              <div class=""info-box"">
                <strong>Order #</strong>{order.OrderId}
                <strong style=""margin-top:8px"">Status</strong><span class=""badge"" style=""background:#10b981"">Delivered</span>
                <strong style=""margin-top:8px"">Delivered At</strong>{order.UpdatedAt:dd MMM yyyy, hh:mm tt} UTC
              </div>
              <p>If you have any concerns about your delivery, please don't hesitate to reach out to our support team.</p>
              <p style=""font-size:16px; font-weight:600; color:#10b981;"">Thank you for choosing PharmacyApp. Stay healthy! 💊</p>";

            return BaseTemplate("Order Delivered – Thank You!", "#10b981", body);
        }
    }
}

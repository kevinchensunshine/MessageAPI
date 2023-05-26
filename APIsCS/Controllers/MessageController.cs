using APIsCS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APIsCS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly MessagesDbContext _context;

        public MessageController(ILogger<MessageController> logger, [FromServices] MessagesDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        [HttpPost("message")]
        public IActionResult PostMessage(Message message)
        {
            _context.Messages.Add(message);
            _logger.LogInformation(_context.SaveChanges() + " elements written into database");
            return Ok();
        }

        static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages.txt");

        [HttpGet("message/{email}")]
        public ActionResult<Message> GetMessage(string searchTerm)
        {
            //searches for email
            _logger.LogInformation("Received search term: {SearchTerm}", searchTerm);

            try
            {
                var matchingMessages = _context.Messages
                    .Where(m => m.Name.Contains(searchTerm) || m.Email.Contains(searchTerm))
                    .ToList();

                if (matchingMessages.Count > 0)
                {
                    return Ok(matchingMessages);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + " " + e.StackTrace);
                return StatusCode(500); // Internal Server Error
            }
        }

        [HttpDelete("message/{email}")]
        public IActionResult DeleteMessage(string email)
        {
            //searches for email
            _logger.LogInformation("Received search term: {SearchTerm}", email);

            try
            {
                var matchingMessages = _context.Messages
                    .Where(m => m.Name.Contains(email) || m.Email.Contains(email));

                if (matchingMessages.Any())
                {
                    _context.Messages.RemoveRange(matchingMessages);
                    _context.SaveChanges();
                    return Ok("Deleted: " + matchingMessages);
                }
                else
                {
                    return Ok("Email not found in database");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + " " + e.StackTrace);
                return StatusCode(500); // Internal Server Error
            }
        }

        Message EditMessage(Message original, Message new_message)
        {
            original.Name = new_message.Name;
            original.Email = new_message.Email;
            original.Content = new_message.Content;
            return original;
        }

        [HttpPut("message")]
        public IActionResult PutMessage(Message message)
        {
            //searches for email
            _logger.LogInformation("Received message", message);

            string email = message.Email;
            string name = message.Name;

            try
            {
                var matchingMessages = _context.Messages
                    .Where(m => m.Name.Contains(name) || m.Email.Contains(email)).ToList();

                if (matchingMessages.Any())
                {
                    foreach (Message matched_message in matchingMessages) {
                        matched_message.Name = message.Name;
                        matched_message.Email = message.Email;
                        matched_message.Content = message.Content;
                        _context.SaveChanges();
                    }
                    return Ok("Changed: " + matchingMessages);
                }
                else
                {
                    return Ok("Email not found in database");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + " " + e.StackTrace);
                return StatusCode(500); // Internal Server Error
            }
        }
    }
}

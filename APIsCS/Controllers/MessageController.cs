﻿using APIsCS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APIsCS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        public MessageController(ILogger<MessageController> logger)
        {
            _logger = logger;
        }


        [HttpPost("message")]
        public IActionResult PostMessage(Message message)
        {
            // Store the message in a text file
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages.txt");
            string messageString = JsonSerializer.Serialize(message);
            System.IO.File.AppendAllText(filePath, messageString + Environment.NewLine);

            // Return a success response
            return Ok();
        }

        static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "messages.txt");

        [HttpGet("message/{email}")]
        public ActionResult<Message> GetMessage(string email)
        {
            //searches for email
            const Int32 BufferSize = 256;
            _logger.LogInformation("Received email: {Email}", email);
            var fileStream = System.IO.File.OpenRead(filePath);
            var streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8, true, BufferSize);

            string? line;

            while ((line = streamReader.ReadLine()) != null)
            {
                try
                {
                    Message? existingMessage = JsonSerializer.Deserialize<Message>(line);
                    if (existingMessage == null)
                    {
                        _logger.LogInformation("Found Null Line\n");
                        continue;
                    }
                    if (existingMessage.Email == email)
                    {
                        _logger.LogInformation("Found line\n");
                        return existingMessage;
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message + " " + e.StackTrace);
                }

            }

            fileStream.Close();
            streamReader.Close();
        
            return NotFound();
        }

        [HttpDelete("message/{email}")]
        public IActionResult DeleteMessage(string email)
        {
            //searches for email
            const Int32 BufferSize = 256;
            _logger.LogInformation("Received email: {Email}", email);
            var fileStream = System.IO.File.OpenRead(filePath);
            var streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8, true, BufferSize);
            string tempFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp_messages.txt");
            var sw = new StreamWriter(tempFile);

            string? line;
            bool flag = false;
            try
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    try
                    {
                        Message? existingMessage = JsonSerializer.Deserialize<Message>(line);
                        if (existingMessage == null)
                        {
                            _logger.LogInformation("Found Null Line\n");
                            continue;
                        }
                        if (existingMessage.Email == email)
                        {
                            _logger.LogInformation("Found and deleting line\n");
                            flag = true;
                            continue;
                        }
                        else
                        {
                            sw.WriteLine(line);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message + " " + e.StackTrace);
                    }

                }

                fileStream.Close();
                streamReader.Close();
                sw.Close();

                System.IO.File.Delete(filePath);
                System.IO.File.Move(tempFile, filePath);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + " " + e.StackTrace);
            }

            return flag ? Ok() : NotFound();
        }

        [HttpPut("message")]
        public IActionResult PutMessage(Message message)
        {
            string email = message.Email;
            const Int32 BufferSize = 256;

            _logger.LogInformation("Received email: {Email}", email);

            var fileStream = System.IO.File.OpenRead(filePath);
            var streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8, true, BufferSize);
            string tempFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp_messages.txt");

            var sw = new StreamWriter(tempFile);

            string? line;
            bool flag = false;

            while ((line = streamReader.ReadLine()) != null)
            {
                try
                {
                    Message? existingMessage = JsonSerializer.Deserialize<Message>(line);
                    if (existingMessage == null)
                    {
                        _logger.LogInformation("Empty message");
                        continue;
                    }
                    if (existingMessage.Email == email)
                    {
                        _logger.LogInformation("Found and replacing line\n");
                        string messageString = JsonSerializer.Serialize(message);
                        sw.Write(messageString + Environment.NewLine);
                        flag = true;
                        continue;
                    }
                    else
                    {
                        sw.WriteLine(line);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message + " " + e.StackTrace);
                }
            }
            try
            {
                fileStream.Close();
                streamReader.Close();
                sw.Close();

                System.IO.File.Delete(filePath);
                System.IO.File.Move(tempFile, filePath);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + " " + e.StackTrace);
            }

            return flag ? Ok() : NotFound();
        }
    }
}
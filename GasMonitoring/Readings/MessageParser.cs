using System;
using System.Collections.Generic;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using SnsMessage = Amazon.SimpleNotificationService.Util.Message;

namespace GasMonitoring.Readings
{
    public class MessageParser
    {
        public Reading ParseMessage(Message message)
        {
            var snsMessage = SnsMessage.ParseMessage(message.Body);
            List<KeyValuePair<string, string>> readingsFromText = new List<KeyValuePair<string, string>>();

            var messageText = JsonConvert.SerializeObject(snsMessage.MessageText);
            var splitMessage = messageText.Split(",");
            foreach (var pair in splitMessage)
            {
                var keyNum = 1;
                var key = "";
                var val = "";
                
                var keyValuePairSplit = pair.Split(":");
                foreach (var kv in keyValuePairSplit)
                {
                    var count = 0;
                    foreach (var character in kv)
                    {
                        if(Char.IsLetterOrDigit(character))
                        {
                            break;
                        }
                        count++;
                    }
                  
                    var substring = kv.Substring(count);
                    
                    var countEnd = 0;
                    foreach (var characterEnd in substring)
                    {
                        if(!Char.IsLetterOrDigit(characterEnd))
                        {
                            break;
                        }
                        countEnd++;
                    }
                    var subSubstring = substring.Substring(0, countEnd);
                    if (keyNum % 2 == 0)
                    {
                        val = subSubstring;
                        keyNum++;
                    } else
                    {
                        key = subSubstring;
                        keyNum++;
                    }
                }
                KeyValuePair<string, string> readingPair = new KeyValuePair<string, string>(key: key, value: val);
                readingsFromText.Add(readingPair);
            }
            
            Reading reading = new Reading
            {
            };
            foreach (var pair in readingsFromText)
            {
                if (pair.Key == "locationId")
                {
                    reading.LocationId = pair.Value;
                } else if (pair.Key == "eventId")
                {
                    reading.EventId = pair.Value;
                } else if (pair.Key == "value")
                {
                    reading.Value = int.Parse(pair.Value);
                } else if (pair.Key == "timestamp")
                {
                    reading.Timestamp = long.Parse(pair.Value);
                }
            }
            return reading;
        }
    }
}
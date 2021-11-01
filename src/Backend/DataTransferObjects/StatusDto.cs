﻿using Backend.Models;
using System;

namespace Backend.DataTransferObjects
{
    public class StatusDto
    {
        public DateTime UpdateTime { get; set; }

        public string Message { get; set; }

        internal Status GetStatus()
        {
            return new Status
            {
                UpdateTime = UpdateTime,
                Message = Message,
            };
        }
    }
}
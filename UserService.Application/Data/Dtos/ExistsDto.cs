﻿namespace UserService.Application.Data.Dtos
{
    public class ExistsDto
    {
        public bool Found { get; set; }
        public Guid? Data { get; set; }
    }
}

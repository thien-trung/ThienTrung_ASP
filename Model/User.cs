﻿namespace TranThienTrung2122110179.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }

        //Thêm cột mới
        public string Address { get; set; }
        public string Description { get; set; }
    }
}

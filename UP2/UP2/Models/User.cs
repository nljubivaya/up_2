﻿using System;
using System.Collections.Generic;

namespace UP2.Models;

public partial class User
{
    public int Userid { get; set; }

    public string Usersurname { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Userpatronymic { get; set; } = null!;

    public string Userlogin { get; set; } = null!;

    public string Userpassword { get; set; } = null!;

    public int Userrole { get; set; }

    public virtual Role UserroleNavigation { get; set; } = null!;
}

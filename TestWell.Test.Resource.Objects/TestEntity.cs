﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWell.Test.Resource.Objects
{
    [Table("TestTable")]
    public class TestEntity
    {
        #region Public Properties

        [Key, Column("Key")]
        public int Id { get; set; }

        [Column("Value")]
        public string Value { get; set; }

        #endregion Public Properties
    }
}

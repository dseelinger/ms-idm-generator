namespace IdmGenerateModels
{
    public static class Templates
    {

        public const string SingleValuedStringFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public string {4}
        {{
            get {{ return GetAttrValue(""{3}""); }}
            set {{
                {5}SetAttrValue(""{3}"", value); 
            }}
        }}

";

        public const string SingleValuedStringFormatTests = @"
        [Fact]
        public void It_can_get_and_set_{0}()
        {{
            // Act
            _it.{0} = ""{1}"";

            // Assert
            _it.{0}.Should().Be(""{1}"");
        }}

{2}";

        public const string NonMatchTest = @"        [Fact]
        public void It_throws_when_{0}_set_to_invalid_value()
        {{
            // Act
            Action action = () => _it.{0} = @""{1}"";
            action.ShouldThrow<ArgumentException>();
        }}

";

        public const string MultiValuedStringFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public List<string> {4}
        {{
            get {{ return GetAttrValues(""{3}""); }}
            set {{
                {5}SetAttrValues(""{3}"", value); 
            }}
        }}

";

        public const string MultiValuedStringFormatTests = @"{1}
        [Fact]
        public void It_can_get_and_set_{0}()
        {{
            var subObject1 = ""foo1"";
            var subObject2 = ""foo2"";
            var list = new List<string> {{ subObject1, subObject2 }};

            // Act
            _it.{0} = list; 

            // Assert
            _it.{0}[0].Should().Be(""foo1"");
            _it.{0}[1].Should().Be(""foo2"");
        }}

";

        public const string MultiValuedStringFormatEmptyTests = @"
        [Fact]
        public void It_has_{0}_which_is_an_empty_collection_by_default()
        {{
            // Assert
            _it.{0}.Should().BeEmpty();
        }}

        [Fact]
        public void It_has_{0}_which_as_a_collection_cannot_be_set_to_null()
        {{
            // Assert
            Action action = () => _it.{0} = null;
            action.ShouldThrow<InvalidOperationException>();
        }}
";

        public const string MultiValuedIntegerFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public List<int> {4}
        {{
            get {{ return GetAttr(""{3}"").Values.Select(int.Parse).ToList(); }}
            set {{
                {5}SetAttrValues(""{3}"", value.Select(v => v.ToString());
            }}
        }}

";

        public const string MultiValuedDateTimeFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public List<DateTime> {4}
        {{
            get {{ return GetAttr(""{3}"").Values.Select(DateTime.Parse).ToList(); }}
            set {{ SetAttrValues(""{3}"", value.Select(v => v.ToString()); }}
        }}

";

        public const string SingleValuedValueFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public {4} {5}
        {{
            get {{ return {6}(""{3}""); }}
            set {{ 
                {7}SetAttrValue(""{3}"", value.ToString());
            }}
        }}

";

        public const string SingleValuedValueFormatTests = @"{2}
        [Fact]
        public void It_can_get_and_set_{0}()
        {{
            // Act
            _it.{0} = {1};

            // Assert
            _it.{0}.Should().Be({1});
        }}

{3}";

        public const string MultiValuedValueFormatTests = @"{2}
        [Fact]
        public void It_can_get_and_set_{0}()
        {{
            // Arrange
            var subObject1 = {1};
            var subObject2 = {1};
            var list = new List<int> {{ subObject1, subObject2 }};

            // Act
            _it.{0} = list;

            // Assert
            _it.{0}[0].Should().Be(""foo1"");
            _it.{0}[1].Should().Be(""foo2"");
        }}

{3}";

        public const string MultiValuedDateTimeFormatTests = @"{1}
        [Fact]
        public void It_can_get_and_set_{0}()
        {{
            // Arrange
            var now = DateTime.Now;
            var testTime1 = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var later = DateTime.Now.AddHours(1);
            var testTime2 = new DateTime(later.Year, later.Month, later.Day, later.Hour, later.Minute, later.Second);

            // Act
            _it.{0}[0] = testTime1;
            _it.{0}[1] = testTime2;

            // Assert
            _it.{0}[0].Should().Be(testTime1);
            _it.{0}[1].Should().Be(testTime2);
        }}

";

        public const string SingleValuedDateTimeFormatTests = @"{1}
        [Fact]
        public void It_can_get_and_set_{0}()
        {{
            // Act
            var now = DateTime.Now;
            var testTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            _it.{0} = testTime;

            // Assert
            _it.{0}.Should().Be(testTime);
        }}

";

        public const string SingleValuedValueNullTestFormat = @"
        [Fact]
        public void It_has_{0}_which_is_null_by_default()
        {{
            // Assert
            _it.{0}.Should().Be(null);
        }}

        [Fact]
        public void It_has_{0}_which_can_be_set_back_to_null()
        {{
            // Arrange
            _it.{0} = {1};

            // Act
            _it.{0} = null;

            // Assert
            _it.{0}.Should().Be(null);
        }}
";

        public const string MultiValuedValueValueEmptyTestFormat = @"
        [Fact]
        public void It_has_{0}_which_is_an_empty_collection_by_default()
        {{
            // Assert
            _it.{0}.Should().BeEmpty();
        }}

        [Fact]
        public void It_has_{0}_which_as_a_collection_cannot_be_set_to_null()
        {{
            // Assert
            Action action = () => _it.{0} = null;
            action.ShouldThrow<InvalidOperationException>();
        }}
";

        public const string MultiValuedDateTimeEmptyTestFormat = @"
        [Fact]
        public void It_has_{0}_which_is_an_empty_collection_by_default()
        {{
            // Assert
            _it.{0}.Should().BeEmpty();
        }}

        [Fact]
        public void It_has_{0}_which_as_a_collection_cannot_be_set_to_null()
        {{
            // Assert
            Action action = () => _it.{0} = null;
            action.ShouldThrow<InvalidOperationException>();
        }}
";

        public const string MinTest = @"        [Fact]
        public void It_throws_when_{0}_is_too_small()
        {{
            // Act
            Action action = () => _it.{0} = {1};
            action.ShouldThrow<ArgumentException>();
        }}

";

        public const string MinTestMultivalued = @"        [Fact]
        public void It_throws_when_{0}_is_too_small()
        {{
            // Arrange
            var subObject1 = {1};
            var subObject2 = {1};
            var list = new List<int> {{ subObject1, subObject2 }};

            // Act
            Action action = () => _it.{0} = list;
            action.ShouldThrow<ArgumentException>();
        }}

";

        public const string MaxTestMultivalued = @"        [Fact]
        public void It_throws_when_{0}_is_too_big()
        {{
            // Arrange
            var subObject1 = {1};
            var subObject2 = {1};
            var list = new List<int> {{ subObject1, subObject2 }};

            // Act
            Action action = () => _it.{0} = list;
            action.ShouldThrow<ArgumentException>();
        }}

";

        public const string MaxTest = @"        [Fact]
        public void It_throws_when_{0}_is_too_big()
        {{
            // Act
            Action action = () => _it.{0} = {1};
            action.ShouldThrow<ArgumentException>();
        }}

";

        public const string SingleValuedDateTimeNullTestFormat = @"
        [Fact]
        public void It_has_{0}_which_is_null_by_default()
        {{
            // Assert
            _it.{0}.Should().Be(null);
        }}

        [Fact]
        public void It_has_{0}_which_can_be_set_back_to_null()
        {{
            // Arrange
            var now = DateTime.Now;
            var testTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            _it.{0} = testTime;

            // Act
            _it.{0} = null;

            // Assert
            _it.{0}.Should().Be(null);
        }}
";

        public const string SingleValuedDateTimeFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public DateTime{5} {4}
        {{
            get {{ return AttrTo{6}DateTime(""{3}""); }}
            set {{ SetAttrValue(""{3}"", value.ToString()); }}
        }}

";

        public const string SingleValuedBinaryFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public byte[] {4}
        {{
            get {{ return GetAttr(""{3}"") == null ? null : GetAttr(""{3}"").ToBinary(); }}
            set {{ SetAttrValue(""{3}"", value == null ? null : Convert.ToBase64String(value)); }}
        }}

";

        public const string SingleValuedBinaryNullTestFormat = @"
        [Fact]
        public void It_has_{0}_which_is_null_by_default()
        {{
            // Assert
            _it.{0}.Should().BeNull();
        }}

        [Fact]
        public void It_has_{0}_which_can_be_set_back_to_null()
        {{
            // Arrange
            var stringReprentation = @""/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAoHBwkHBgoJCAkLCwoMDxkQDw4ODx4WFxIZJCAmJSMgIyIoLTkwKCo2KyIjMkQyNjs9QEBAJjBGS0U+Sjk/QD3/2wBDAQsLCw8NDx0QEB09KSMpPT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT3/wAARCAAyADIDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD2CS6t42KvPGrDqCwBpv221/5+Yf8AvsV494+O3xpfgZGPL7/9M1rnsn1P51i6tnY+pw/DqrUYVPaW5knt3XqfQDXdqf8Al4h/77FcBoHjKPS9dvNOvmAs3uZDFLj/AFZLHr6g+vb6V59uPrRuO7Oec5zUuo2d+H4fp04ThOXMpeVreZ9DowYZBBUjIIPWnV5Z4L8anTymn6m5a0J2xSt/yy9if7v8vpXqKuGAKkFSMgg9a2jJSR8rjsBUwdTknt0fcfRSUVRwni/j/wD5HXUP+2f/AKLWudrovH//ACOuof8AbP8A9FrXO1yS3Z+nYD/dKX+GP5IKKKKk6xQcGu18GeNTpzJp2pvm0biOVusR9D/s/wAq4quu8F+Dn1iVb29QixjbhT/y2I7fT1NXC99Dzs0jhnh5fWNvxv5ef9bHqI1K1IBE8WP94UUz+ybD/nzg/wC/YorqPz21Lz/A8k8e8eMr/wD7Z/8Aota57Nez6n4L0nVtQlu7qKRppMbiJCBwABx9BVX/AIVxoX/PvL/3+NYSpSbbPrcLn+FpUIU5J3SS27L1PIutBGK9e/4VzoOP9RLn/rsa4rRfB0ms+ILqLDR6fbTMjv3wGPyj3x+VQ6ckd9DOsLWjKaulHe4zwd4RfX7nz7oMlhEfmYcGQ/3R/U16/DBHBCkUKCONAAqqMACmWlpDZ26W9tGI4Y1Cqi9ABU/WuiEeVHx2Y5jPHVOZ6RWy/rqFFLRVHnBRRRQA09DWP4aH/EumPf7TN/6GaKKSOin/AAJ+q/U2aWiimc4UUUUAf//Z"";
            var byteArray = Convert.FromBase64String(stringReprentation);
            _it.{0} = byteArray; 

            // Act
            _it.{0} = null;

            // Assert
            _it.{0}.Should().BeNull();
        }}
";

        public const string MultialuedBinaryEmptyTestFormat = @"
        [Fact]
        public void It_has_{0}_which_is_an_empty_collection_by_default()
        {{
            // Assert
            _it.{0}.Should().BeEmpty();
        }}

        [Fact]
        public void It_has_{0}_which_as_a_collection_cannot_be_set_to_null()
        {{
            // Assert
            Action action = () => _it.{0} = null;
            action.ShouldThrow<InvalidOperationException>();
        }}
";

        public const string SingleValuedBinaryFormatTests = @"{1}
        [Fact]
        public void It_can_get_and_set_{0}()
        {{
            // Arrange
            var stringReprentation = @""/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAoHBwkHBgoJCAkLCwoMDxkQDw4ODx4WFxIZJCAmJSMgIyIoLTkwKCo2KyIjMkQyNjs9QEBAJjBGS0U+Sjk/QD3/2wBDAQsLCw8NDx0QEB09KSMpPT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT3/wAARCAAyADIDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD2CS6t42KvPGrDqCwBpv221/5+Yf8AvsV494+O3xpfgZGPL7/9M1rnsn1P51i6tnY+pw/DqrUYVPaW5knt3XqfQDXdqf8Al4h/77FcBoHjKPS9dvNOvmAs3uZDFLj/AFZLHr6g+vb6V59uPrRuO7Oec5zUuo2d+H4fp04ThOXMpeVreZ9DowYZBBUjIIPWnV5Z4L8anTymn6m5a0J2xSt/yy9if7v8vpXqKuGAKkFSMgg9a2jJSR8rjsBUwdTknt0fcfRSUVRwni/j/wD5HXUP+2f/AKLWudrovH//ACOuof8AbP8A9FrXO1yS3Z+nYD/dKX+GP5IKKKKk6xQcGu18GeNTpzJp2pvm0biOVusR9D/s/wAq4quu8F+Dn1iVb29QixjbhT/y2I7fT1NXC99Dzs0jhnh5fWNvxv5ef9bHqI1K1IBE8WP94UUz+ybD/nzg/wC/YorqPz21Lz/A8k8e8eMr/wD7Z/8Aota57Nez6n4L0nVtQlu7qKRppMbiJCBwABx9BVX/AIVxoX/PvL/3+NYSpSbbPrcLn+FpUIU5J3SS27L1PIutBGK9e/4VzoOP9RLn/rsa4rRfB0ms+ILqLDR6fbTMjv3wGPyj3x+VQ6ckd9DOsLWjKaulHe4zwd4RfX7nz7oMlhEfmYcGQ/3R/U16/DBHBCkUKCONAAqqMACmWlpDZ26W9tGI4Y1Cqi9ABU/WuiEeVHx2Y5jPHVOZ6RWy/rqFFLRVHnBRRRQA09DWP4aH/EumPf7TN/6GaKKSOin/AAJ+q/U2aWiimc4UUUUAf//Z"";
            var byteArray = Convert.FromBase64String(stringReprentation);

            // Act
            _it.{0} = byteArray; 

            // Assert
            _it.{0}[0].Should().Be(byteArray[0]);
            _it.{0}[1].Should().Be(byteArray[1]);
            _it.{0}[2].Should().Be(byteArray[2]);
            _it.{0}[_it.{0}.Length - 1].Should().Be(byteArray[byteArray.Length - 1]);
        }}

";

        public const string MultivaluedBinaryFormatTests = @"{1}
        [Fact]
        public void It_can_get_and_set_{0}()
        {{
            // Arrange
            var stringReprentation = @""/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAoHBwkHBgoJCAkLCwoMDxkQDw4ODx4WFxIZJCAmJSMgIyIoLTkwKCo2KyIjMkQyNjs9QEBAJjBGS0U+Sjk/QD3/2wBDAQsLCw8NDx0QEB09KSMpPT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT3/wAARCAAyADIDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD2CS6t42KvPGrDqCwBpv221/5+Yf8AvsV494+O3xpfgZGPL7/9M1rnsn1P51i6tnY+pw/DqrUYVPaW5knt3XqfQDXdqf8Al4h/77FcBoHjKPS9dvNOvmAs3uZDFLj/AFZLHr6g+vb6V59uPrRuO7Oec5zUuo2d+H4fp04ThOXMpeVreZ9DowYZBBUjIIPWnV5Z4L8anTymn6m5a0J2xSt/yy9if7v8vpXqKuGAKkFSMgg9a2jJSR8rjsBUwdTknt0fcfRSUVRwni/j/wD5HXUP+2f/AKLWudrovH//ACOuof8AbP8A9FrXO1yS3Z+nYD/dKX+GP5IKKKKk6xQcGu18GeNTpzJp2pvm0biOVusR9D/s/wAq4quu8F+Dn1iVb29QixjbhT/y2I7fT1NXC99Dzs0jhnh5fWNvxv5ef9bHqI1K1IBE8WP94UUz+ybD/nzg/wC/YorqPz21Lz/A8k8e8eMr/wD7Z/8Aota57Nez6n4L0nVtQlu7qKRppMbiJCBwABx9BVX/AIVxoX/PvL/3+NYSpSbbPrcLn+FpUIU5J3SS27L1PIutBGK9e/4VzoOP9RLn/rsa4rRfB0ms+ILqLDR6fbTMjv3wGPyj3x+VQ6ckd9DOsLWjKaulHe4zwd4RfX7nz7oMlhEfmYcGQ/3R/U16/DBHBCkUKCONAAqqMACmWlpDZ26W9tGI4Y1Cqi9ABU/WuiEeVHx2Y5jPHVOZ6RWy/rqFFLRVHnBRRRQA09DWP4aH/EumPf7TN/6GaKKSOin/AAJ+q/U2aWiimc4UUUUAf//Z"";
            var byteArray = Convert.FromBase64String(stringReprentation);
            var list = new List<byte[]> {{
                byteArray,
                byteArray
            }};

            // Act
            _it.{0} = list; 

            // Assert
            _it.{0}[0][0].Should().Be(byteArray[0]);
            _it.{0}[0][1].Should().Be(byteArray[1]);
            _it.{0}[0][2].Should().Be(byteArray[2]);
            _it.{0}[1][0].Should().Be(byteArray[0]);
            _it.{0}[1][1].Should().Be(byteArray[1]);
            _it.{0}[1][2].Should().Be(byteArray[2]);
            _it.{0}[0][_it.{0}[0].Length - 1].Should().Be(byteArray[byteArray.Length - 1]);
            _it.{0}[1][_it.{0}[0].Length - 1].Should().Be(byteArray[byteArray.Length - 1]);
        }}

";

        public const string MultiValuedBinaryFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        {2}public List<byte[]> {4}
        {{
            get {{ return GetAttr(""{3}"")?.ToBinaries(); }}
            set {{ SetAttrValues(""{3}"", value?.Select(Convert.ToBase64String).ToList()); }}
        }}

";
        public const string SingleValuedReferenceFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        public {3} {4}
        {{
            get {{ return GetAttr(""{2}"", _the{4}); }}
            set 
            {{ 
                _the{4} = value;
                SetAttrValue(""{2}"", ObjectIdOrNull(value)); 
            }}
        }}
        private {3} _the{4};

";

        public const string SingleValuedReferenceFormatTests = @"
        [Fact]
        public void It_has_{0}_which_is_null_by_default()
        {{
            // Assert
            _it.{0}.Should().Be(null);
        }}

        [Fact]
        public void It_has_{0}_which_can_be_set_back_to_null()
        {{
            // Arrange
            var test{1} = new {1} {{ DisplayName = ""Test {1}"" }};			
            _it.{0} = test{1}; 

            // Act
            _it.{0} = null;

            // Assert
            _it.{0}.Should().Be(null);
        }}

        [Fact]
        public void It_can_get_and_set_{0}()
        {{
            // Act
			var test{1} = new {1} {{ DisplayName = ""Test {1}"" }};			
            _it.{0} = test{1}; 

            // Assert
            _it.{0}.DisplayName.Should().Be(test{1}.DisplayName);
        }}

";

        public const string MultivaluedReferenceFormatTests = @"
        [Fact]
        public void It_has_{0}_which_is_an_empty_collection_by_default()
        {{
            // Assert
            _it.{0}.Should().BeEmpty();
        }}

        [Fact]
        public void It_has_{0}_which_as_a_collection_cannot_be_set_to_null()
        {{
            // Assert
            Action action = () => _it.{0} = null;
            action.ShouldThrow<InvalidOperationException>();
        }}

        [Fact]
        public void It_can_get_and_set_{0}()
        {{
            // Arrange
            var list = new List<{1}>
            {{
                new {1} {{ DisplayName = ""Test {1}1"", ObjectID = ""guid1"" }},
                new {1} {{ DisplayName = ""Test {1}2"", ObjectID = ""guid2"" }}
            }};

            // Act
            _it.{0} = list;

            // Assert
            _it.{0}[0].DisplayName.Should().Be(list[0].DisplayName);
            _it.{0}[1].DisplayName.Should().Be(list[1].DisplayName);
        }}
";

        public const string MultiValuedReferenceFormat = @"
        /// <summary>
        /// {0} - {1}
        /// </summary>
        public List<{3}> {4}
        {{
            get {{ return GetMultiValuedAttr(""{2}"", _the{4}); }}
            set {{ SetMultiValuedAttr(""{2}"", out _the{4}, value); }}
        }}
        private List<{3}> _the{4};

";


        public const string ClassTemplate = @"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
// ReSharper disable InconsistentNaming

namespace IdmNet.Models
{{
    /// <summary>
    /// {0} - {1}
    /// </summary>
    public class {2} : IdmResource
    {{
        /// <summary>
        /// Parameterless CTOR
        /// </summary>
        public {2}()
        {{
            ObjectType = ForcedObjType = ""{0}"";
        }}

        /// <summary>
        /// Build a {2} object from a IdmResource object
        /// </summary>
        /// <param name=""resource"">base class</param>
        public {2}(IdmResource resource)
        {{
            Attributes = resource.Attributes;
            ObjectType = ForcedObjType = ""{0}"";
            if (resource.Creator == null)
                return;
            Creator = resource.Creator;
        }}

        readonly string ForcedObjType;

        /// <summary>
        /// Object Type (can only be {0})
        /// </summary>
        [Required]
        public override sealed string ObjectType
        {{
            get {{ return GetAttrValue(""ObjectType""); }}
            set
            {{
                if (value != ForcedObjType)
                    throw new InvalidOperationException(""Object Type of {2} can only be '{0}'"");
                SetAttrValue(""ObjectType"", value);
            }}
        }}
{3}
    }}
}}
";




        public const string TestClassTemplate = @"using System;
using System.Collections.Generic;
using IdmNet.Models;
using Xunit;
using FluentAssertions;
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable UseObjectOrCollectionInitializer

namespace IdmNet.Models.Tests
{{
    public class {0}Tests
    {{
        private {0} _it;

        public {0}Tests()
        {{
            _it = new {0}();
        }}

        [Fact]
        public void It_has_a_paremeterless_constructor()
        {{
            _it.ObjectType.Should().Be(""{1}"");
        }}

        [Fact]
        public void It_has_a_constructor_that_takes_an_IdmResource()
        {{
            var resource = new IdmResource
            {{
                DisplayName = ""My Display Name"",
                Creator = new Person {{ DisplayName = ""Creator Display Name"", ObjectID = ""Creator ObjectID""}},
            }};
            var it = new {0}(resource);

            it.ObjectType.Should().Be(""{1}"");
            it.DisplayName.Should().Be(""My Display Name"");
            it.Creator.DisplayName.Should().Be(""Creator Display Name"");
        }}

        [Fact]
        public void It_has_a_constructor_that_takes_an_IdmResource_without_Creator()
        {{
            var resource = new IdmResource
            {{
                DisplayName = ""My Display Name"",
            }};
            var it = new {0}(resource);

            it.DisplayName.Should().Be(""My Display Name"");
            it.Creator.Should().Be(null);
        }}

        [Fact]
        public void It_throws_when_you_try_to_set_ObjectType_to_anything_other_than_its_primary_ObjectType()
        {{
            Action action = () => _it.ObjectType = ""Invalid Object Type"";
            action.ShouldThrow<InvalidOperationException>();
        }}
{2}
    }}
}}
";


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonFrontEnd.ControllerModel
{
    /// <summary>
    /// Will contain ETI Format for Order Entry and Login
    /// </summary>
    public class SenderControllerModel
    {
        #region ETI Message Type

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class Messages
        {

            private MessagesMessage[] messageField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Message")]
            public MessagesMessage[] Message
            {
                get
                {
                    return this.messageField;
                }
                set
                {
                    this.messageField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class MessagesMessage
        {

            private MessagesMessageItems[] itemsField;

            private string nameField;

            private ushort numberField;

            private string typeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Items")]
            public MessagesMessageItems[] Items
            {
                get
                {
                    return this.itemsField;
                }
                set
                {
                    this.itemsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort Number
            {
                get
                {
                    return this.numberField;
                }
                set
                {
                    this.numberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class MessagesMessageItems
        {

            private string nameField;

            private string typeField;

            private string sourceField;

            private string valueField;

            private int positionField;

            private bool positionFieldSpecified;

            private ushort lengthField;

            private bool lengthFieldSpecified;

            private string dependencyPropertyField;

            private string mappedPropertyField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Source
            {
                get
                {
                    return this.sourceField;
                }
                set
                {
                    this.sourceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int Position
            {
                get
                {
                    return this.positionField;
                }
                set
                {
                    this.positionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool PositionSpecified
            {
                get
                {
                    return this.positionFieldSpecified;
                }
                set
                {
                    this.positionFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort Length
            {
                get
                {
                    return this.lengthField;
                }
                set
                {
                    this.lengthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool LengthSpecified
            {
                get
                {
                    return this.lengthFieldSpecified;
                }
                set
                {
                    this.lengthFieldSpecified = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DependencyProperty
            {
                get
                {
                    return this.dependencyPropertyField;
                }
                set
                {
                    this.dependencyPropertyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string MappedProperty
            {
                get
                {
                    return this.mappedPropertyField;
                }
                set
                {
                    this.mappedPropertyField = value;
                }
            }
        }

        #endregion

        #region ReturnedOrderMessageStructure


        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class ReturnedOrderMessages
        {

            private ReturnedOrderMessagesFilter[] messageFiltersField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Filter", IsNullable = false)]
            public ReturnedOrderMessagesFilter[] MessageFilters
            {
                get
                {
                    return this.messageFiltersField;
                }
                set
                {
                    this.messageFiltersField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ReturnedOrderMessagesFilter
        {

            private ReturnedOrderMessagesFilterCode[] codeField;

            private string typeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Code")]
            public ReturnedOrderMessagesFilterCode[] Code
            {
                get
                {
                    return this.codeField;
                }
                set
                {
                    this.codeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ReturnedOrderMessagesFilterCode
        {

            private ReturnedOrderMessagesFilterCodeItems[] itemsField;

            private ushort replyCodeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Items")]
            public ReturnedOrderMessagesFilterCodeItems[] Items
            {
                get
                {
                    return this.itemsField;
                }
                set
                {
                    this.itemsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort ReplyCode
            {
                get
                {
                    return this.replyCodeField;
                }
                set
                {
                    this.replyCodeField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ReturnedOrderMessagesFilterCodeItems
        {

            private ushort messageTypeField;

            private string messageField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort MessageType
            {
                get
                {
                    return this.messageTypeField;
                }
                set
                {
                    this.messageTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Message
            {
                get
                {
                    return this.messageField;
                }
                set
                {
                    this.messageField = value;
                }
            }
        }



        #endregion
    }
}

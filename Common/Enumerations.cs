using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace CommonFrontEnd.Common
{
    public static partial class Enumerations
    {
        public enum SocketCompression
        {
            NoCompression,
            Encoding,
            LZOandEncoding,
            LZOCompression
        }
        public enum ConnectionType
        {
            LAN,
            Internet,
            LeasedLine,
            DR
        }


      

        public enum Side
        {
            Buy = 1,
            Sell = 2
        }
        public enum CurIdxStk
        {
            ALL,
            CURR,
            INDEX,
            STOCK
        }

        #region ColourProfiling

        public enum WindowsAvailable
        {
            Touchline,
            PendingOrder,
            Trades,
            TimeAndSales
        }

        public enum TouchlineColor
        {
            BackgroundColor,
            Uptrend,
            Downtrend,
            QuantityRate,
            ForeGroundColor,
            UptrendFlash,
            DowntrendFlash
        }

        public enum PendingOrderColor
        {
            BackgroundColor,
            BuyOrder,
            SellOrder,
            BuyStoplossOrder,
            SellStoplossOrder,
            RRMBuyOrder,
            RRMSellOrder
        }

        public enum TradesColor
        {
            BackgroundColor,
            BuyTrade,
            SellTrade,
            BuySpreadTrade,
            SellSpreadTrade,
            SpreadBackgroundColor
        }

        public enum TimeAndSalesColor
        {
            ScripListBackground,
            ScripListUptrend,
            ScripListDowntrend,
            ScripDataBackground,
            ScripDataUptrend,
            ScripDataDowntrend
        }



        #endregion


        #region OrderEnumerations
        public static class Order
        {
            #region TWS Specific
            public enum OrdStatus
            {
                New = 0,
                PartiallyFilled = 1,
                Filled = 2,
                Cancelled = 4,
                Suspended = 9
            }
            /// <summary>
            /// ProductComplex/InstrumentType 
            ///1- Simple Instrument,
            ///2- Standard Option Strategy,
            ///3- Non-Standard Option Strategy,
            ///4- Options Volatility Strategy and
            ///5- Futures Spread
            /// </summary>
            public enum ProductComplex
            {
                SimpleInstrument = 1,
                StandardOptionStrategy = 2,
                NonStandardOptionStrategy = 3,
                OptionsVolatilityStrategy = 4,
                FuturesSpread = 5
            }
            /// <summary>
            /// ExecType
            ///0- New,
            ///4- Cancelled,
            ///5- Replaced,
            ///9- Suspended,
            ///D- Restated,
            ///L- Triggered,
            ///F- Trade,
            ///M- RRM Order Accept,
            ///N- RRM Order Reject,
            ///X- Provisional Accept,
            ///Y- Provisional Order Reject,
            /// </summary>
            public enum ExecType
            {
                New = '0',
                Cancelled = '4',
                Replaced = '5',
                Suspended = '9',
                Restated = 'D',
                Triggered = 'L',
                Trade = 'F',
                RRMOrderAccept = 'M',
                RRMOrderReject = 'N',
                ProvisionalAccept = 'X',
                ProvisionalOrderReject = 'Y'
            }
            /// <summary>
            /// Triggered
            ///0- Not Triggered,
            ///1- Triggered Stop Order,
            ///2- Triggered OCO Order,
            /// </summary>
            public enum Triggered
            {
                NotTriggered = 0,
                TriggeredStopOrder = 1,
                TriggeredOCOOrder = 2
            }

            /// <summary>
            /// ExecRestatementReason
            /// </summary>


            #endregion
            public enum CMBuySellParameters
            {
                Token = 0,
                NseToken = 1,
                BseToken = 2,
                Destination = 3,
                Market = 4,
                Symbol = 5,
                Series = 6,
                Volume = 7,
                Price = 8,
                TriggerPrice = 9,
                DisclosedVolume = 10,
                BackOfficeId = 11,
                Delivery = 12,
                BuySellIndicator = 13,
                AlphaChar = 14,
                ActualApproverId = 15,
                VolumeRemaining = 16,
                DisclosedVolumeRemaining = 17,
                AdminUsId = 18,
                UserId = 19,
                LoginId = 20,
                BatchOrder = 21,
                ProClientIndicator = 22,
                ParticipantCode = 23,
                IntuitionalClient = 24,
                GoodTillDate = 25,
                IOC = 26,
                TransactionCode = 27,
                OrderId = 28,
                OrderNumber = 29,
                Source = 30,
                BookType = 31,
                RowState = 32,
                Segment = 33,
                Group = 34,
                BlockDeal = 35,
                Reason = 36,
                ExcelFlag = 37,
                SolicitorPeriod = 38,
                NO_OF_PARAMETERS = 39
            }


            public enum FNOBuySellParameters
            {
                Token = 0,
                NseToken = 1,
                Destination = 2,
                Market = 3,
                InstrumentType = 4,
                Symbol = 5,
                ExpiryDate = 6,
                StrikePrice = 7,
                OptionType = 8,
                Volume = 9,
                Price = 10,
                TriggerPrice = 11,
                GoodTillDate = 12,
                VolumeRemaining = 13,
                BuySellIndicator = 14,
                UserId = 15,
                LoginId = 16,
                BackOfficeId = 17,
                GoodTillCancel = 18,
                IOC = 19,
                ProClientIndicator = 20,
                ParticipantCode = 21,
                TransactionCode = 22,
                OrderId = 23,
                OrderNumber = 24,
                Source = 25,
                RowState = 26,
                OrderType = 27,
                ClientType = 28,
                Settlor = 29,
                DisclosedVolume = 30,
                UserRemarks = 31,
                Name = 32,
                Reason = 33,
                Series = 34,
                ExcelFlag = 35,
                SolicitorPeriod = 36,
                BseToken = 37,
                Delivery = 38,
                QtyMCX = 39
            }

            public enum ScripSegment
            {

                Equity = 1,
                Derivative = 2,
                Currency = 3,
                Debt = 8,
                //#if BOW

                Commodities = 11,
                MutualFund = 5,
                SLB = 6,
                OFS = 7,
                DebtTo = 9,
                ITP = 10
                //#endif
            };
            public enum InstrumentType
            {
                Call,
                Put,
                Future,
                PairOption,
                Straddle
            };            
            public enum DervInstrumentType
            {
                FutIndex = 1,
                FutStock = 2,
                CallIndex = 3,
                CallStock = 4,
                PutIndex = 5,
                PutStock = 6,
                PairOption = 7
            };
            public enum CurrInstrumentType
            {
                Future = 1,
                Call = 2,
                Put = 3,
                PairOption = 4,
                Straddle = 5
            };
            public enum Exchanges
            {
                BSE = 1,
                //#if BOW
                NSE = 2,
                USE = 7,
                BSEINX = 9,
                NCDEX = 3,
                NMCE = 4,
                MCX = 5,
                DGCX = 8
                //#endif
            };
            public enum OrderTypes
            {
#if BOW
                LIMIT = 0,
                MARKET = 1,
                STOPLOSS = 2,
                OCO = 3,
#elif TWS
                LIMIT = 0,
                MARKET = 1,
                STOPLOSS = 2,
                STOPLOSSMKT = 2,
                BLOCKDEAL = 6,
                OCO = 3
                //BOC = 10
#endif
            };

            public enum OrderTypesBatch
            {
#if TWS
                LIMIT_L,
                MKT_G,
                OCO_L,
                SL_P,
                SLMKT_P,
                BDEAL_K,
                ODDL_O
#endif
            };

            public enum OrderTypeExchange
            {
                LIMIT = 76,
                MARKET = 71,
                BLOCKDEAL = 75,
                QUOTES = 81,
                STOPLOSS = 80
            }
            public enum RetType
            {
#if BOW
                EOTODY = 2,
                EOS = 1,
                EOSTLM = 3
#elif TWS
                //EOTODY = 2,
                //EOS = 1,
                //IOC = 4
                IOC = 0,
                EOS = 1,
                EOD = 2
#endif
            };
            public enum ClientTypes
            {
#if BOW
                CLIENT,
                SPLCLI,
                INST,
                OWN
#elif TWS
                OWN = 20,
                CLIENT = 30,
                SPLCLI = 40,
                INST = 90,
#endif
            };

            public enum BuySellFlag
            {
                B = 0,
                S = 1
                //as per BOW
                //BUY = 0,
                //SELL = 1
            };

            public enum ApplSeqIndicator
            {
                LeanOrder = 0,
                StandardOrder = 1
            };

            public enum PriceValidityCheckType
            {
                None = 0
            };


            //Instructions for order handling.
            public enum ExecInst
            {
                PersistentOrder = 1,
                NonpersistentOrder = 2,
                PersistentBOCOrder = 5,
                NonpersistentBOCOrder = 6
            };

            public enum TradingSessionSubID
            {
                ClosingAuction = 4
            };

            public enum TradingCapacity
            {
                Customer_Agency = 1,
                Principal_Proprietary = 5,
                Market_Maker = 6
            }

            public enum PositionEffect
            {
                //Close
                C,
                //Open
                O
            }

            public enum Modes
            {
#if BOW
                None = 0,
                Add = 1,
                Edit = 2,
                Arbitrage = 3
#elif TWS
                A = 1,//Add
                U = 2,//Update
                D = 4//Delete
#endif

            };

            public enum OrderConfirmation
            {
                MessageIdentifier = 0,
                OETransactionCode = 1,
                OEID = 2,
                OEExchange = 3,
                OEMarket = 4,
                OEToken = 5,
                OELogTime = 6,
                OEErrorCode = 7,
                OEOrderNumber = 8,
                OEBookType = 9,
                OEVolume = 10,
                OEVolumeRemaining = 11,
                OEDisclosedVolume = 12,
                OEDisclosedVolumeRemaining = 13,
                OEVolumeFilledToday = 14,
                OEPrice = 15,
                OETriggerPrice = 16,
                OEFlags = 17,
                OEBroker = 18,
                OETraderId = 19,
                OEBranchId = 20,
                OERemarks = 21,
                OEEntryDateTime = 22,
                OELastModifiedDateTime = 23,
                OEParticipantType = 24,
                OEModifiedCancelledBy = 25,
                OEReasonCode = 26,
                OEGoodTillDate = 27,
                OEProClientIndicator = 28,
                OEAdminUsId = 29,
                OEAdminUSBackOfficeId = 30,
                OEReason = 31,
                OEStatus = 32,
                OEExpectedApproverId = 33,
                OEExpectedApproverBackOfficeId = 34,
                OEActualApproverId = 35,
                OEActualApproverBackOfficeId = 36,
                OEApproverRemarks = 37,
                OEOldVolume = 38,
                OEOldPrice = 39,
                OEErrorMessage = 40,
                OECreatedBy = 41,
                OECreatedByLoginId = 42,
                OECreatedAt = 43,
                OELastUpdatedBy = 44,
                OELastUpdatedByLoginId = 45,
                OELastUpdatedAt = 46,
                OEFIELD1 = 47,
                OEFIELD2 = 48,
                OEFIELD3 = 49,
                OEFIELD4 = 50,
                OEROWSTATE = 51,
                OEUsId = 52,
                OELoginId = 53,
                Size = 53
            };
        }
        #endregion
        public static class Trade
        {
            public enum AdminTradeRequestMsgTag
            {
                EQTYSEGINDICATOR = 601,//Equity
                DERISEGINDICATOR = 602,// Derivatives
                CURRSEGINDICATOR = 603, // Currency
                BOLTSEGINDICATOR = 604, // BOLT
                CMSEQTYSEGINDICATOR = 605,// BOLT
                CMSEBOLTSEGINDICATOR = 606, // BOLT
            }
        }

        public enum OrderExecutionStatus
        {
            NA,//Order sent
            Exits,//Pending i.e after order resonse
            Executed,//fully executed
            Return,//Exchange return
            Deleted,//user deleted
            StopExist,
            RstopExist,
            OrderCancelled_Return,//special case: trade then 3233 OR 3233 then trades
            Batch
        }

        #region Column Profiling
        public enum WindowName
        {
            Exchange_Default_Profile = 0,
            Touchline = 1,
            Pending_Order = 2,
            Trade = 3,
            Batch_Order = 4,
            Swift_OE = 5,
            Normal_OE = 6,
            Fast_OE = 7,
            Pending_OE = 8,
            Stoploss_OE = 9
        }
        #endregion

        #region TradeEnumerations
#if BOW
        public enum Trade
        {
            TRTransactionCode = 1,
            TRID = 2,
            TRExchange = 3,
            TRMarket = 4,
            TRToken = 5,
            TRLogTime = 6,
            TRErrorCode = 7,
            TRNumber = 8,
            TRVolume = 9,
            TRPrice = 10,
            TROrderNumber = 11,
            TRRemainingVolume = 12,
            TRDisclosedVolumeRemaining = 13,
            TRVolumeFilledToday = 14,
            TRTradeTime = 15,
            TRAdminUSId = 16,
            TRAdminUSBackOfficeId = 17,
            TRCreatedBy = 18,
            TRCreatedByLoginId = 19,
            TRCreatedAt = 20,
            TRLastUpdatedBy = 21,
            TRLastUpdatedByLoginId = 22,
            TRLastUpdatedAt = 23,
            TRFIELD1 = 24,
            TRFIELD2 = 25,
            TRFIELD3 = 26,
            TRFIELD4 = 27,
            TRROWSTATE = 28,
            TRErrorMessage = 29,
            TROEID = 30,
            TRNewUsId = 31,
            TRNewBackOfficeId = 32,
            TROldParticipantCode = 33,
            TRNewParticipantCode = 34,
            Size = 34
        }
#endif
        #endregion

        #region TabOrderScripHelp
        public enum ScripHelpTab
        {
            ScripTab = 0,
            ColorTab = 1,
            BoltSettingTab = 3,
            OrdersTab = 4,
            coloumnTab = 5,
            //ClientTab = 5,
            EmailProfilingTab = 7,
            FunctionKeysTab = 8,
            ThemesTab = 9
        };
        #endregion
    }
#if TWS

    public static partial class Enumerations
    {
        #region ETI Structure No Value
        public enum SignedInt : ulong
        {
            OneByte = 127,
            TwoByte = 32767,
            FourByte = 2147483647,
            EightByte = 9223372036854775807
        };

        public enum USignedInt : ulong
        {
            OneByte = 255,
            TwoByte = 65535,
            FourByte = 4294967295,
            EightByte = 18446744073709551615
        };

        public enum Float : ulong
        {
            EightByte = 9223372036854775807
        };


        public static class FixedString
        {
            public const string FirstPosition = "\0";
        };

        public enum FixedStringTerminable
        {
            FirstPosition = 0
        };

        public enum VariableString
        {
            FirstPosition = 0
        };

        public enum Counter
        {
            OneByte = 255,
            TwoByte = 65535
        }

        public enum LocalMktDate : ulong
        {
            FourByte = 4294967295
        }

        public enum PriceType : ulong
        {
            EightByte = 9223372036854775807
        }

        public enum Qty : ulong
        {
            FourByte = 2147483647
        }

        public enum SeqNum : ulong
        {
            EightByte = 18446744073709551615
        }

        public enum UTCTimestamp : ulong
        {
            EightByte = 18446744073709551615
        }

        public enum Char
        {
            OneByte = 0
        }

        public enum Data
        {
            EachByte = 0
        }

        public enum chartext
        {
            FirstPosition = 0
        }
        #endregion

        #region Common Messaging Window
        public struct MsgCat
        {
            public const string SelectAll = "SelectAll";
            public const string Request = "Request";
            public const string Reply = "Reply";
            public const string Ums = "UMS";
            public const string News = "News";
            public const string Other = "Other";
            public const string Email = "Email";
        }

        public struct MsgPrefix
        {
            public const string SelectAll = "SelectAll";
            public const string Ord = "Ord";
            public const string Sl = "SL";
            public const string Rect = "Rect";
            public const string Trd = "Trd";
            public const string Qry = "Qry";
            public const string Rrm = "RRM";
            public const string News = "News";
            public const string Alert = "Alert";
            public const string Other = "Other";
        }

        public struct MsgClassCode
        {
            public const short Request = 1;
            public const short Reply = 2;
            public const short Ums = 3;
            public const short News = 4;
            public const short Other = 5;
        }

        public struct RegexStrings
        {
            public const string BoltTime = "^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5]*[0-9]:[0-5]*[0-9]";
        }
        #endregion

        #region Common Enums

        public enum KeystrokeForOE
        {
            Enter,
            EnterControl

        }

        public enum Environment
        {
            Live = 0,
            Test = 1,
            QA = 2
        }

        public enum Role
        {
            Admin = 0,
            Trader = 1
        }

        public enum OrderEntryWindow
        {
            Normal = 0,
            Swift = 1,
            Fast = 2
        }

        public enum Segment
        {
            //Equity = 1,
            //Derivative = 2,
            //Currency = 3,
            //Debt = 4
            //After Changes in values
            //Equity = 1,
            //Derivative = 2,
            //Currency = 4,
            //Debt = 8,
            //Commodities = 3,
            //SLB = 6,
            //ITP = 10
            Equity = 0,
            Derivative = 1,
            Currency = 2,
            Debt = 3,
            Commodities = 4,
            SLB = 5,
            ITP = 6


#if BOW
                
                Commodities = 3,
                MutualFund = 5,
                SLB = 6,
                OFS = 7,
                DebtTo = 9,
                ITP = 10
#endif
        }



        public enum Exchange
        {
            //BSE,
            //NSE,
            //USE,
            //BSEINX 
            BSE = 1,
            NSE = 2,
            NCDEX = 3,
            MCX = 5
#if BOW
                NSE = 2,
                USE = 7,
                BSEINX = 9,
                NCDEX = 3,
                NMCE = 4,
                MCX = 5,
                DGCX = 8
#endif
        }

        public enum AccountType
        {
            OWN = 20,
            CLIENT = 30,
            SPLCLI = 40,
            INST = 90,
        };



        public enum SideShort
        {
            B = 1,
            S = 2
        }

        public enum OrderType
        {
            Limit = 2,
            Market = 5,
            BlockDeal = 6,
            StopMarket = 3,
            StopLimit = 4
        }

        /// <summary>
        /// Used on Returned order screen
        /// </summary>
        public enum ReturnedOrderReason
        {
            EOSSESS = 0,
            PCAS = 1,
            RRM = 2,
            SPOS = 3,
            OTHER = 4,
            MASSCANCELL = 5
        }

        public enum OrderTypeShort
        {
            L = 2,
            G = 5,
            K = 6,
            SLMkt = 3,
            SL = 4
        }

        public enum DefaultFocusforNormalOE
        {
            ScripId,
            ScripCode,
            OrderQty,
            Rate
        }

        public enum DefaultFocusForSwiftOE
        {
            ScripId,
            ScripCode,
            OrderQty,
            Rate
        }

        public enum DefaultFocusForMultilegOE
        {
            Segment,
            Legs,
            BuyorSell
        }

        public enum TimeInForce
        {
            Day = 0,
            IOC = 3,
            Ses = 7
        }

        public enum FutureOption
        {
            All,
            Future,
            Option
        }

        public enum CallPut
        {
            All,
            Call,
            Put
        }

        #region DefOrderEntry            
        public enum DefOrderEntry
        {
            Normal,
            Fast,
            Swift
           // Multilegged
        };



        //#region DefaultFocus
        //public enum DefaultFocus
        //{


        //};

        //#endregion



        #endregion

        public enum HFLFFlag
        {
            HFFlag = 1,
            LFFlag = 2
        }

        public enum OrderTypeDownload
        {
            NormalOrders = 1092,
            StopLossOrders = 1097,
            ReturnOrders = 1170,
            ReturnStopLossOrders = 1173
        }
        public enum TradeTypeDownload
        {
            TradeRequest = 1095,
            TradeEnhancementRequest = 1080
        }
        #endregion


        //#region Column Profiling
        //    public enum WindowName
        //    {
        //        Select = 0,
        //        Touchline = 1,
        //        Pending_Order = 2,
        //        Trade = 3
        //    }
        //#endregion


    }
#elif BOW
    public static partial class Enumerations
    {
        public enum Exchange
        {
            BSE,
            NSE,
            USE,
            BSEINX,
            NCDEX,
            MCX
        }
        public enum Segment
        {
            Equity = 1,
            Derivative = 2,
            Currency = 3,
            Debt = 4,
            Commodities
        }
        public enum FIELDS
        {
            ID = 0,
            USID = 1,
            Name = 2,
            SegmentId = 3,
            Format = 4,
            CreatedBy = 5,
            CreatedAt = 6,
            LastUpdatedBy = 7,
            LastUpdatedAt = 8,
            FIELD1 = 9,
            FIELD2 = 10,
            FIELD3 = 11,
            FIELD4 = 12,
            ROWSTATE = 13,
            Type = 14,
            Admin = 15,
            NoOfLegs = 16,
            SortType = 17,
            SortFrequency = 18
        }

        public enum DefaultFocusforNormalOE
        {
            ScripId,
            ScripCode,
            OrderQty,
            Rate
        }

        public enum DefaultFocusForSwiftOE
        {
            ScripId,
            ScripCode,
            OrderQty,
            Rate
        }

        public enum SortDirection
        {
            NONE = 0,
            ASCENDING = 1,
            DESENDING = 2,
        }
        public enum MWROW_FIELDS
        {
            Id = 0,
            Exchange = 1,
            Market = 2,
            Token = 3,
            Rowstate = 4,
            YPos = 5,
            PositionOnScreen = 6,
            SIZE = 7
        }
        public enum DefOrderEntry
        {

            Fast,
            Swift,
            Multilegged,
            Normal
        }


        public enum DefaultFocusForMultilegOE
        {
            Segment,
            Legs,
            BuyorSell
        }


        public enum SortType
        {
            NoSort = 1,
            FixedPeriodBased = 2,
            OnEveryTick,
        }
        public enum MktByPrice
        {
            MessageIdentifier = 0,
            Time = 1,
            Exchange = 2,
            Segment = 3,
            CommonToken = 4,
            InstrumentType = 7,
            Symbol = 8,
            Series = 9,
            ExpiryDate = 10,
            StrikePrice = 11,
            OptionType = 12,
            VolumeTradedToday = 13,
            LastTradePrice = 14,
            NetChangeIndicator = 15,
            NetChange = 16,
            NetChangePercentage = 17,
            LastTradeQty = 18,
            LastTradeTime = 19,
            AvgTradePrice = 20,

            Quantity1 = 21,
            Price1 = 22,
            NumberOfOrders1 = 23,
            Quantity2 = 24,
            Price2 = 25,
            NumberOfOrders2 = 26,
            Quantity3 = 27,
            Price3 = 28,
            NumberOfOrders3 = 29,
            Quantity4 = 30,
            Price4 = 31,
            NumberOfOrders4 = 32,
            Quantity5 = 33,
            Price5 = 34,
            NumberOfOrders5 = 35,

            Quantity6 = 36,
            Price6 = 37,
            NumberOfOrders6 = 38,
            Quantity7 = 39,
            Price7 = 40,
            NumberOfOrders7 = 41,
            Quantity8 = 42,
            Price8 = 43,
            NumberOfOrders8 = 44,
            Quantity9 = 45,
            Price9 = 46,
            NumberOfOrders9 = 47,
            Quantity10 = 48,
            Price10 = 49,
            NumberOfOrders10 = 50,
            TotalBuyQty = 51,
            TotalSellQty = 52,
            ClosePrice = 53,
            OpenPrice = 54,
            HighPrice = 55,
            LowPrice = 56,
            Low52 = 57,
            High52 = 58,

            VALUETRADEDTODAY = 59,
            NUMBEROFTRADESTODAY = 60,
            UPPERCIRCUIT = 61,
            LOWERCIRCUIT = 62,

            BuyAvgPrice = 63,
            SellAvgPrice = 64,

            LastUpdateTime = 65,
            TotalTradedValue = 66,
            DailyPriceRange = 67,

            OpenInterest = 68,
            CA = 69,
            MarketLot = 70,
            Numerator = 71,
            Denominator = 72,
            IndicativeQtyPrice = 73,
            Size = 74
        }

        public enum MBPMessageCalledFrom
        {
            MBP = 1,
            NetPosition = 2,
            QuoteEntry = 3,
            UserAlert = 4,
            OnlineMTMPL = 5,
            Excel = 6,
            Chart = 7,
            IndexControl = 8,
            NetPosition_TradeWise = 9,
            OrderSlicing = 10,
            SensexFileLog = 11,
            MultiLegOrderEntry = 12
        }

    }
#endif

}

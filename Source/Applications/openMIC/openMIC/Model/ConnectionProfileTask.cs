//******************************************************************************************************
//  ConnectionProfileTask.cs - Gbtc
//
//  Copyright � 2017, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  02/10/2016 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GSF.ComponentModel;
using GSF.Configuration;
using GSF.Data.Model;
using GSF.TimeSeries.Adapters;
using Newtonsoft.Json;

namespace openMIC.Model
{
    // Defines connection profile task settings
    public class ConnectionProfileTaskSettings
    {
        private string m_fileExtensions;
        private string[] m_fileSpecs;

        [ConnectionStringParameter,
        Description("Defines file names or patterns to download."),
        DefaultValue("*.*")]
        public string FileExtensions
        {
            get
            {
                return m_fileExtensions;
            }
            set
            {
                m_fileExtensions = value;
                m_fileSpecs = null;
            }
        }

        public string[] FileSpecs
        {
            get
            {
                return m_fileSpecs ?? (m_fileSpecs = (m_fileExtensions ?? "*.*").Split(',').Select(pattern => pattern.Trim()).ToArray());
            }
        }

        [ConnectionStringParameter,
        Description("Defines remote path to download files from ."),
        DefaultValue("/")]
        public string RemotePath
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Defines local path to download files to."),
        DefaultValue("")]
        public string LocalPath
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Determines if remote folders should scanned for matching downloads - file structure will be replicated locally."),
        DefaultValue(false)]
        public bool RecursiveDownload
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Determines if remote files should be deleted after download."),
        DefaultValue(false)]
        public bool DeleteRemoteFilesAfterDownload
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Determines if total remote files to download should be limited by age."),
        DefaultValue(false)]
        public bool LimitRemoteFileDownloadByAge
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Determines if old local files should be deleted."),
        DefaultValue(false)]
        public bool DeleteOldLocalFiles
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Determines if download should be skipped if local file already exists and matches remote."),
        DefaultValue(false)]
        public bool SkipDownloadIfUnchanged
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Determines if existing local files should be overwritten."),
        DefaultValue(false)]
        public bool OverwriteExistingLocalFiles
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Determines if existing local files should be archived before new ones are downloaded."),
        DefaultValue(false)]
        public bool ArchiveExistingFilesBeforeDownload
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Determines if downloaded file timestamps should be synchronized to remote file timestamps."),
        DefaultValue(true)]
        public bool SynchronizeTimestamps
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Defines external operation application."),
        DefaultValue("")]
        public string ExternalOperation
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Defines maximum amount of time, in seconds, to allow the external operation to sit idle."),
        DefaultValue(null)]
        public double? ExternalOperationTimeout
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Defines maximum file size to download."),
        DefaultValue(1000)]
        public int MaximumFileSize
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Defines maximum file count to download."),
        DefaultValue(-1)]
        public int MaximumFileCount
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Defines directory naming expression."),
        DefaultValue("<YYYY><MM>\\<DeviceFolderName>")]
        public string DirectoryNamingExpression
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Defines directory authentication user name."),
        DefaultValue("")]
        public string DirectoryAuthUserName
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Defines directory authentication password."),
        DefaultValue("")]
        public string DirectoryAuthPassword
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Determines if an e-mail should be sent if the downloaded files have been updated."),
        DefaultValue(false)]
        public bool EmailOnFileUpdate
        {
            get;
            set;
        }

        [ConnectionStringParameter,
        Description("Defines the recipient e-mail addresses to use when sending e-mails on file updates."),
        DefaultValue("")]
        public string EmailRecipients
        {
            get;
            set;
        }
    }

    public class ConnectionProfileTask
    {
        #region [ Members ]

        // Fields
        private bool m_success;

        #endregion

        #region [ Constructors ]

        public ConnectionProfileTask()
        {
            Settings = new ConnectionProfileTaskSettings();
        }

        #endregion

        #region [ Properties ]

        [PrimaryKey(true)]
        public int ID
        {
            get;
            set;
        }

        public int ConnectionProfileID
        {
            get;
            set;
        }

        [Required]
        [StringLength(200)]
        public string Name
        {
            get;
            set;
        }

        [FieldName("Settings")]
        public string ConnectionString
        {
            get
            {
                ConnectionStringParser<ConnectionStringParameterAttribute> parser = new ConnectionStringParser<ConnectionStringParameterAttribute>();
                return parser.ComposeConnectionString(Settings);
            }
            set
            {
                ConnectionStringParser<ConnectionStringParameterAttribute> parser = new ConnectionStringParser<ConnectionStringParameterAttribute>();
                parser.ParseConnectionString(value, Settings);
            }
        }

        [NonRecordField]
        [JsonIgnore]
        public ConnectionProfileTaskSettings Settings
        {
            get;
        }

        [DefaultValueExpression("DateTime.UtcNow")]
        public DateTime CreatedOn
        {
            get;
            set;
        }

        [Required]
        [StringLength(200)]
        [DefaultValueExpression("UserInfo.CurrentUserID")]
        public string CreatedBy
        {
            get;
            set;
        }

        [DefaultValueExpression("this.CreatedOn", EvaluationOrder = 1)]
        [UpdateValueExpression("DateTime.UtcNow")]
        public DateTime UpdatedOn
        {
            get;
            set;
        }

        [Required]
        [StringLength(200)]
        [DefaultValueExpression("this.CreatedBy", EvaluationOrder = 1)]
        [UpdateValueExpression("UserInfo.CurrentUserID")]
        public string UpdatedBy
        {
            get;
            set;
        }

        [NonRecordField]
        [JsonIgnore]
        public bool Success => m_success;

        #endregion

        #region [ Methods ]

        public void Reset()
        {
            m_success = true;
        }

        public void Fail()
        {
            m_success = false;
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseOnlineTradingSystem
{
    /// <summary>
    /// 登录响应
    /// </summary>
    public class LoginResponse: BaseResponse
    {
        public LoginInfo data;
    }

    /// <summary>
    /// 登录信息
    /// </summary>
    public class LoginInfo
    {
        public string created_at;
        public string email;
        public string id;
        public string last_login;
        public string mobile;
        public QQData qq;
        public string status;
        public SubSystemData subsystem;
        public string ticket;
        public string ticket_id;
        public string username;
        public WeiXinData weixin;
    }

    /// <summary>
    /// 附加信息
    /// </summary>
    public class SubSystemData
    {
        public string admin;
        public string api_domain;
        public string id;
        public string label;
        public string login_setting;
        public string name;
        public string remark;
        public string valid_time;
    }
    /// <summary>
    /// QQ数据
    /// </summary>
    public class QQData
    { }

    /// <summary>
    /// 微信数据
    /// </summary>
    public class WeiXinData
    { }


    /// <summary>
    /// 个人信息响应
    /// </summary>
    public class MineResponse : BaseResponse
    {
        public string timeStamp;
        public Mine data;
    }

    /// <summary>
    /// 个人信息
    /// </summary>
    public class Mine
    {
        public string lastLoginTime;
        public string address;
        public string addressCity;
        public string addressDistrict;
        public string addressProvince;
        public string birthday;
        public List<Company> companyList;
        public Company currentCompany;

        public string driverLicenceBackFile;
        public string driverLicenceBackImage;
        public string driverLicenceCode;
        public string driverLicenceFaceFile;
        public string driverLicenceFaceImage;
        public string driverLicenceType;
        public string email;
        public string gender;
        public string headImage;
        public string headImageInfo;
        public string id;
        public string identity;
        public string identityBackFile;
        public string identityBackImage;
        public string identityExpiryDate;
        public string identityFaceFile;
        public string identityFaceHandFile;
        public string identityFaceHandImage;
        public string identityFaceImage;
        public string identityStartDate;
        public string identityType;
        public string loginName;
        public string mobile;
        public string moduleUid;
        public string name;
        public string otherContact;
        public string professionQualificationBackFile;
        public string professionQualificationBackImage;
        public string professionQualificationCode;
        public string professionQualificationFaceFile;
        public string professionQualificationFaceImage;
        public string professionQualificationType;
        public string regChannel;
        public string remark;
        public string salt;
        public string status;
        public string userType;
        public string userTypeText;
        public string vehicleId;
        public string verifyDate;
        public string verifyMessage;
        public string verifyStatus;
        public string loginDevice;
        public string passportUserId;
        public string ukeyFlag;
    }

    /// <summary>
    /// 公司信息
    /// </summary>
    public class Company
    {
        public string id;
        public string createdDate;
        public string createdBy;
        public string updatedDate;
        public string updatedBy;
        public string deleteFlag;
        public string companyType;
        public string companyName;
        public string organizationOrCreditCode;
        public string busLicenceCode;
        public string corporationName;
        public string generalBusinessScope;
        public string preBusinessScope;
        public string companyOpenAccountName;
        public string companyOpenAccountBank;
        public string companyOpenAccount;
        public string managerName;
        public string managerDeptJob;
        public string managerMobile;
        public string managerTelephone;
        public string managerEmail;
        public string managerIdentity;
        public string managerIdentityFaceFileId;
        public string managerIdentityBackFileId;
        public string applyOfficialLetterId;
        public string companyCredentialsType;
        public string businessLicenseId;
        public string verifyStatus;
        public string taxationRegId;
        public string organizationCodeId;
        public string otherProveFileId;
        public string creditCodeFile;
        public string modifyFlag;
        public string modifyTime;
        public string addressCity;
        public string addressProvince;
        public string addressDistrict;
        public string companyShortName;
        public string companyAddress;
        public string contactPerson;
        public string contactNumber;
        public string phoneNumber;
        public string fax;
        public string email;
        public string qq;
        public string busLicenceFile;
        public string organizationFile;
        public string taxationRegCode;
        public string taxationRegFile;
        public string corporationIdentity;
        public string corporationIdentityFaceFile;
        public string corporationIdentityBackFile;
        public string merchantNo;
        public string openAccountFile;
        public string openAccountCode;
        public string shortcode;
        public string verifyMessage;
        public string singleOrgCertificationCode;
        public string singleOrgCertificationFile;
        public string mixed3to1;
        public string busLicenceContents;
        public string organizationContents;
        public string taxationRegContents;
        public string moduleUid;
        public string corporationPhone;
        public string corporationTel;
        public string verifyDate;
        public string authenticationFile;
        public string credit;
        public string level;
        public string registeredCapital;
        public string companyNature;
        public string foundingDate;
        public string billingInformation;
        public string companyUrl;
        public string shareholderInfo;
        public string operatingPeriodStartDate;
        public string operatingPeriodEndDate;
        public string companyLogo;
        public string transId;
        public string transStatus;
        public string remark;
        public string blackListStatus;
        public string whiteListStatus;
        public string relStatus;
        public string position;
        public string companyOwner;
        public string lastChoice;
    }
}

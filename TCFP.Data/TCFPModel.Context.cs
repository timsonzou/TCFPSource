﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TCFP.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Common;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TCFPEntities : DbContext
    {
        public TCFPEntities()
        : base("name=TCFPEntities")
    	{
    	}
    
    	public TCFPEntities(DbConnection existingConnection, bool contextOwnsConnection)
    		: base(existingConnection, contextOwnsConnection)
    	{
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<SystemParameter> SystemParameters { get; set; }
        public virtual DbSet<ClientToken> ClientTokens { get; set; }
        public virtual DbSet<ClientProfile> ClientProfiles { get; set; }
    
        public virtual int sp_RegisterNewClient(string email, string name, string tokenID)
        {
            var emailParameter = email != null ?
                new ObjectParameter("email", email) :
                new ObjectParameter("email", typeof(string));
    
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var tokenIDParameter = tokenID != null ?
                new ObjectParameter("tokenID", tokenID) :
                new ObjectParameter("tokenID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_RegisterNewClient", emailParameter, nameParameter, tokenIDParameter);
        }
    
        public virtual ObjectResult<sp_GetUserToken_Result> sp_GetUserToken(string tokenID)
        {
            var tokenIDParameter = tokenID != null ?
                new ObjectParameter("tokenID", tokenID) :
                new ObjectParameter("tokenID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetUserToken_Result>("sp_GetUserToken", tokenIDParameter);
        }
    
        public virtual int sp_ResetPassword(string tokenID, string password)
        {
            var tokenIDParameter = tokenID != null ?
                new ObjectParameter("tokenID", tokenID) :
                new ObjectParameter("tokenID", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("password", password) :
                new ObjectParameter("password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_ResetPassword", tokenIDParameter, passwordParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> sp_Login(string email, string password)
        {
            var emailParameter = email != null ?
                new ObjectParameter("email", email) :
                new ObjectParameter("email", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("password", password) :
                new ObjectParameter("password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("sp_Login", emailParameter, passwordParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> sp_ForgetPassword(string email, string tokenID)
        {
            var emailParameter = email != null ?
                new ObjectParameter("email", email) :
                new ObjectParameter("email", typeof(string));
    
            var tokenIDParameter = tokenID != null ?
                new ObjectParameter("tokenID", tokenID) :
                new ObjectParameter("tokenID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("sp_ForgetPassword", emailParameter, tokenIDParameter);
        }
    }
}

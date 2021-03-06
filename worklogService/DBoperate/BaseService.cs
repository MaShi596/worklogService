﻿
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using worklogService.Models;

namespace worklogService.DBoperate
{
    public class BaseService
    {

        ISessionFactory factory;

        string ip;
        string uid;
        string dbname;
        string pwd;

        ISession OpenSession()
        {
            if (factory == null)
            {
                var cgf = new Configuration();
                var data = cgf.Configure(
                      HttpContext.Current.Server.MapPath("/") +
                         @"Models\NHibernate\Configuration\hibernate.cfg.xml");
                cgf.AddDirectory(new System.IO.DirectoryInfo(
                      HttpContext.Current.Server.MapPath("/")+@"Models\NHibernate\Mappings"));
                factory = data.BuildSessionFactory();
            }
            return factory.OpenSession();
        } 

        public BaseService()
        {


            //if (factory == null)
            //{
            //    var cgf = new Configuration();
            //    var data = cgf.Configure(
            //          HttpContext.Current.Server.MapPath(
            //             @"Models\NHibernate\Configuration\hibernate.cfg.xml"));
            //    cgf.AddDirectory(new System.IO.DirectoryInfo(
            //          HttpContext.Current.Server.MapPath(@"Models\NHibernate\Mappings")));
            //    factory = data.BuildSessionFactory();
            //}
            //factory.OpenSession();
            //factory = Connection.getConfiguration().BuildSessionFactory();
        }


        //public object saveEntity(BaseEntity entity)
        //{
        //    object newentity = new object();
        //    if (entity.Id != 0)
        //    {
        //        //throw new Exception("id不为空,应使用update方法");
        //        // MessageBox.Show("保存失败！");
        //        return null;
        //    }
        //    ISessionFactory factory;
        //    ISession session = null;
        //    ITransaction transaction = null;
        //    // Tell NHibernate that this object should be updated
        //    try
        //    {
        //        factory = Connection.getConfiguration().BuildSessionFactory();
        //        session = factory.OpenSession();
        //        transaction = session.BeginTransaction();
        //        newentity = session.Save(session.Merge(entity));
        //        // commit all of the changes to the DB and close the ISession
        //        transaction.Commit();
        //        // MessageBox.Show("保存成功");
        //        return newentity;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //        //if (transaction != null && transaction.IsActive)
        //        //{
        //        // transaction.Rollback();
        //        //}
        //        //MessageBox.Show("保存失败！");
        //    }
        //    finally
        //    {
        //        if (session != null && session.IsOpen)
        //        {
        //            session.Close();
        //        }
        //    }
        //}


        ///// <summary>
        /////删除实体 
        ///// </summary>
        ///// <param name="entity"></param>
        //public void deleteEntity(BaseEntity entity)
        //{
        //    if (entity.Id == 0)
        //    {
        //        //MessageBox.Show("删除失败！");
        //        return;
        //    }
        //    ISessionFactory factory;
        //    ISession session = null;
        //    ITransaction transaction = null;
        //    // Tell NHibernate that this object should be updated
        //    try
        //    {
        //        factory = Connection.getConfiguration().BuildSessionFactory();
        //        session = factory.OpenSession();
        //        transaction = session.BeginTransaction();
        //        session.Delete(session.Merge(entity));
        //        // commit all of the changes to the DB and close the ISession
        //        transaction.Commit();
        //        //MessageBox.Show("删除成功");
        //    }
        //    catch
        //    {
        //        if (transaction != null && transaction.IsActive)
        //        {
        //            transaction.Rollback();
        //        }
        //        //MessageBox.Show("删除失败！");
        //    }
        //    finally
        //    {
        //        if (session != null && session.IsOpen)
        //        {
        //            session.Close();
        //        }
        //    }
        //}

        /// <summary>
        /// 保存或更新实体
        /// </summary>
        /// <param name="entity"></param>
        public void SaveOrUpdateEntity(BaseEntity entity)
        {
            //ISessionFactory factory = null;
            ISession session = null;
            ITransaction transaction = null;
            // Tell NHibernate that this object should be updated
            try
            {
                //factory = Connection.getConfiguration().BuildSessionFactory();
                session = OpenSession();//factory.OpenSession();
                transaction = session.BeginTransaction();
                session.SaveOrUpdate(session.Merge(entity));
                // commit all of the changes to the DB and close the ISession
                transaction.Commit();
                //MessageBox.Show("保存成功");
            }
            catch (Exception e)
            {
                if (transaction != null && transaction.IsActive)
                {
                    transaction.Rollback();
                }
                throw e;
            }
            finally
            {
                if (session != null && session.IsOpen)
                {
                    factory.Close();
                    session.Clear();
                    session.Close();
                    factory.Dispose();
                    session.Dispose();
                }
            }
        }


        public BaseEntity loadEntity(BaseEntity entity, long id)
        {
            if (id == 0)
            {
                return null;
            }
            try
            {

                ISession session = null;
                //ISessionFactory factory = null;

                //if (factory == null)


                session = OpenSession();//factory.OpenSession();


                ITransaction transaction = session.BeginTransaction();
                session.Load(entity, id);
                session.Close();
                return entity;
            }
            catch
            {
                return null;
            }
        }


        ///// <summary>
        ///// 查询实体List
        ///// </summary>
        ///// <param name="entityNames">实体名称字符串数组</param>
        ///// <param name="colAndValue">查询条件二维数组.0为实体名称，1为属性名称，2为属性类型是否为字符串，3为属性值.
        ///// 例：new string[,]{{"Course","CourseName",CommonStaticParameter.IS_STRING,"aaa"}}</param>
        ///// <returns>查询多个实体的时候返回Object[]的List,查询单个实体的时候返回实体对象数组</returns>
        //public ArrayList loadEntityList(List<string> entityNames, string[,] colAndValue)
        //{
        //    try
        //    {
        //        ISessionFactory factory = Connection.getConfiguration().BuildSessionFactory();
        //        ISession session = factory.OpenSession();
        //        ITransaction trans = session.BeginTransaction();
        //        if (entityNames.Count <= 0)
        //        {
        //            return null;
        //        }
        //        StringBuilder sql = new StringBuilder("select ").Append("{").Append(entityNames[0]).Append(".*}");
        //        for (int i = 1; i < entityNames.Count; i++)
        //        {
        //            sql.Append(",{").Append(entityNames[i]).Append(".*}");
        //        }
        //        sql.Append(" from ").Append(entityNames[0]);
        //        for (int i = 1; i < entityNames.Count; i++)
        //        {
        //            sql.Append(",").Append(entityNames[i]);
        //        }
        //        if (colAndValue.Length >= 4)
        //        {
        //            sql.Append(" where ");
        //            if (!colAndValue[0, 0].Equals(""))
        //            {
        //                sql.Append(colAndValue[0, 0]).Append(".");
        //            }
        //            sql.Append(colAndValue[0, 1]).Append("=");
        //            if (colAndValue[0, 2].Equals(CommonStaticParameter.IS_STRING))
        //            {
        //                sql.Append("'").Append(colAndValue[0, 3]).Append("'");
        //            }
        //            else if (colAndValue[0, 2].Equals(CommonStaticParameter.IS_NOT_STRING))
        //            {
        //                sql.Append(colAndValue[0, 3]);
        //            }
        //            for (int i = 1; i < (colAndValue.Length / 4); i++)
        //            {
        //                sql.Append(" and ");
        //                if (!colAndValue[i, 0].Equals(""))
        //                {
        //                    sql.Append(colAndValue[i, 0]).Append(".");
        //                }
        //                sql.Append(colAndValue[i, 1]).Append("=");
        //                if (colAndValue[i, 2].Equals(CommonStaticParameter.IS_STRING))
        //                {
        //                    sql.Append("'").Append(colAndValue[i, 3]).Append("'");
        //                }
        //                else if (colAndValue[i, 2].Equals(CommonStaticParameter.IS_NOT_STRING))
        //                {
        //                    sql.Append(colAndValue[i, 3]);
        //                }
        //            }
        //        }
        //        ISQLQuery query = session.CreateSQLQuery(sql.ToString());
        //        for (int i = 0; i < entityNames.Count; i++)
        //        {
        //            query.AddEntity("ClassLibrary." + entityNames[i]);
        //        }
        //        ArrayList list = (ArrayList)query.List();
        //        session.Close();
        //        return list;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //public ArrayList loadEntityList(List<string> entityNames, string sql)
        //{
        //    try
        //    {
        //        ISessionFactory factory = Connection.getConfiguration().BuildSessionFactory();
        //        ISession session = factory.OpenSession();
        //        ITransaction trans = session.BeginTransaction();
        //        ISQLQuery query = session.CreateSQLQuery(sql);
        //        for (int i = 0; i < entityNames.Count; i++)
        //        {
        //            query.AddEntity("ClassLibrary." + entityNames[i]);
        //        }
        //        ArrayList list = (ArrayList)query.List();
        //        session.Close();
        //        return list;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public IList loadEntityList(string sql)
        {
            ISession session = null;
            //ISessionFactory factory = null;
            try
            {
                //if (factory == null)


                session = OpenSession();//factory.OpenSession();
                ITransaction trans = session.BeginTransaction();
                IQuery query = session.CreateQuery(sql);
                IList result = query != null ? query.List() : null;
                session.Close();
                return result;
            }
            catch (Exception e)
            {
                throw e;
                //return null;
            }
            finally
            {
                if (session != null && session.IsOpen)
                {
                    session.Clear();
                    session.Close();
                    factory.Close();
                    factory.Dispose();
                    session.Dispose();
                }
            }
        }


        private  SqlConnection connection = null;

        public  SqlConnection getSqlConnection()
        {

            //var cgf = new Configuration();
            //var data = cgf.Configure(
            //      HttpContext.Current.Server.MapPath("/") +
            //         @"Models\NHibernate\Configuration\hibernate.cfg.xml");

             //ip = 
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(HttpContext.Current.Server.MapPath("/") +
                     @"Models\NHibernate\Configuration\hibernate.cfg.xml");
            XmlNodeList connstringnode = xdoc.GetElementsByTagName("property");
            string connstring = connstringnode[3].InnerText.Trim();
            string[] connel = connstring.Split(new char[] { ';' });
            
            foreach(string o in connel)
            {

                if (o.Contains("UID"))
                {
                    uid = o.Substring(o.IndexOf('=') + 1, o.Length - o.IndexOf('=') - 1);
                }
                else if (o.Contains("PWD"))
                {
                
                    pwd = o.Substring(o.IndexOf('=') + 1, o.Length - o.IndexOf('=') - 1);
                }
                else if (o.Contains("Database"))
                { 
                    dbname = o.Substring(o.IndexOf('=') + 1, o.Length - o.IndexOf('=') - 1);
                }

                else if (o.Contains("server"))
                {

                    ip = o.Substring(o.IndexOf('=') + 1, o.Length - o.IndexOf('=') - 1);
                }
            }
            
            //xdoc.GetElementById
            //XmlNode xnode = xdoc.DocumentElement.FirstChild;
            //XmlNodeList xnodelist = xnode.ChildNodes;
            //XmlNode connstring = xnodelist.


            if (connection == null || connection.State == ConnectionState.Closed)
            {

                connection = new SqlConnection(connstring);
            }
            if (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                }
                catch
                {
                    throw;
                }
            }
            return connection;
        }



        /// <summary>
        /// 执行sql语句，不是hql
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList ExecuteSQL(string query)
        {



            IList result = new ArrayList();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                conn = getSqlConnection();
                cmd = new SqlCommand(query, conn);

                SqlDataReader rs = cmd.ExecuteReader();
                while (rs.Read())
                {
                    int fieldCount = rs.FieldCount;
                    Object[] values = new Object[fieldCount];
                    for (int i = 0; i < fieldCount; i++)
                        values[i] = rs.GetValue(i);
                    result.Add(values);
                }
            }
            catch (Exception e)
            {
                throw e;

            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return result;
        }


        /// <summary>
        /// 监测数据库数据更新情况
        /// </summary>
        /// <param name="changeMethod">dependency_OnChange(object sender, SqlNotificationEventArgs e)</param>
        /// <param name="sql">select ID,UserID,[Message] From [dbo].[Messages]</param>
        public  void autoUpdateForm(OnChangeEventHandler changeMethod, String sql)
        {
            try
            {
                using (SqlConnection connection = getSqlConnection())
                {
                    //依赖是基于某一张表的,而且查询语句只能是简单查询语句,不能带top或*,同时必须指定所有者,即类似[dbo].[]
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        SqlDependency dependency = new SqlDependency(command);
                        dependency.OnChange += changeMethod;
                        SqlDataReader objReader = command.ExecuteReader();
                    }
                }
            }
            catch
            {
                return;
            }
        }


    }
}
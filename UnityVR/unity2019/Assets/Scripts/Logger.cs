﻿/**
@brief Experimental logger class

:copyright: Copyright 2013-2023, see AUTHORS.
:license: GPLv3, see LICENSE for details.
**/


using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;

public class Logger
{

    //private string LogFileName = "TimeLog.txt";
    private static volatile Logger instance;
    private static object syncRoot = new System.Object();
    private static object syncLog = new System.Object();
    private StringBuilder sb = new StringBuilder();

    private Logger()
    {

    }

    public static Logger Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new Logger();
                }
            }

            return instance;
        }
    }

    ~Logger()
    {
        /*
        using (StreamWriter outfile = new StreamWriter(LogFileName))
        {
            outfile.Write(sb.ToString());
        }
         */
    }



    public static void SaveToFile(string filename)
    {
        lock (syncRoot)
        {
            using (StreamWriter outfile = new StreamWriter(filename))
            {
                outfile.Write(instance.sb.ToString());                
            }
            instance.sb.Length = 0;
        }
    }

    public void Log(string nr, DateTime date, TimeSpan timeSpan, byte[] image = null)
    {
        StringBuilder s = new StringBuilder();
        DateTimeOffset dateOffsetValue = new DateTimeOffset(date);
        if (image == null)
        {
            s.AppendFormat("{0};{1};{2}", nr, timeSpan.TotalMilliseconds, dateOffsetValue.ToString("hh:mm:ss.fff"));
        }
        else
        {
            s.AppendFormat("{0};{1};{2};{3}", nr, timeSpan.TotalMilliseconds, dateOffsetValue.ToString("hh:mm:ss.fff"), image[0]);
        }
        s.AppendLine("");
        Log(s.ToString());

    }

    public void Log(string nr, DateTime date, byte[] image = null)
    {
        StringBuilder s = new StringBuilder();

        DateTimeOffset dateOffsetValue = new DateTimeOffset(date);
        if (image == null)
        {
            s.AppendFormat("{0};;{1}", nr, dateOffsetValue.ToString("hh:mm:ss.fff"));
        }
        else
        {
            s.AppendFormat("{0};;{1};{2}", nr, dateOffsetValue.ToString("hh:mm:ss.fff"), image[0]);
        }
        s.AppendLine("");
        Log(s.ToString());
    }

    public void Log(string x)
    {
        lock (syncLog)
        {
            sb.Append(x);
        }
    }



}

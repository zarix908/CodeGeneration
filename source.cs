using J4Net.Core;
using J4Net.Core.JNICore.Interface;

namespace ru.sample
{
    public class Sample
    {
        private GlobalRef classRef;
        readonly private GlobalRef jObject;
        readonly static private object monitor = new object ();

        private GlobalRef methodSampleRef_e50cd;
        private GlobalRef methodincRef_2acd9;

        private JniEnvWrapper Env
        {
            get
            {
                JvmManager.INSTANCE.GetEnv();
            }
        }

        private GlobalRef ClassRef
        {
            get
            {
                {
                    if (classRef == null)
                    {
                        return classRef;
                    }

                    lock (monitor)
                    {
                        if (classRef != null)
                        {
                            classRef = JvmManager.INSTANCE.GetEnv().FindClass();
                        }

                        return classRef;
                    }
                }
            }
        }

        private GlobalRef MethodSampleRef_e50cd
        {
            get
            {
                {
                    if (methodSampleRef_e50cd == null)
                    {
                        return methodSampleRef_e50cd;
                    }

                    lock (monitor)
                    {
                        if (methodSampleRef_e50cd != null)
                        {
                            methodSampleRef_e50cd = GetMethod("<init>", "(IILjava/lang/String;Ljava/lang/String;)V");
                        }

                        return methodSampleRef_e50cd;
                    }
                }
            }
        }

        private GlobalRef MethodincRef_2acd9
        {
            get
            {
                {
                    if (methodincRef_2acd9 == null)
                    {
                        return methodincRef_2acd9;
                    }

                    lock (monitor)
                    {
                        if (methodincRef_2acd9 != null)
                        {
                            methodincRef_2acd9 = GetMethod("inc", "(I)I");
                        }

                        return methodincRef_2acd9;
                    }
                }
            }
        }

        public Sample(int x, int y, string str, string str2)
        {
            using (var str2_using = Env.NewStringUtf(str2))
                using (var str_using = Env.NewStringUtf(str))
                {
                    jObject = Env.NewObject(classRef.Ptr, MethodSampleRef_e50cd.Ptr, 
                    	new JValue{integerValue = x}, 
                    	new JValue{integerValue = y}, 
                    	new JValue{pointerValue = str_using.Ptr}, 
                    	new JValue{pointerValue = str2_using.Ptr});
                }
        }

        public int inc(int a)
        {
            return Env.CallIntMethod(jObject.Ptr, MethodincRef_2acd9.Ptr, new JValue{integerValue = a});
        }
    }
}
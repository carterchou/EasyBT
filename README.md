![image](https://github.com/FierceMalaymo/EasyBT/blob/615951b5af061b6445466ef25dcefdbb8dc75380/%E6%9C%AA%E5%91%BD%E5%90%8D.png)

# EasyBT

   A free visual behavior tree editing tool for UNITY.
   It can help you create any behavior of game objects quickly.
   
   Support 2021.3.15f1 version and above.
   
   Gmail:qwe654100@gmail.com
   
   # How to work?
   https://youtube.com/playlist?list=PLVDO-bv4pl99ey0GchsuxhYP-i8erhM80
   
   # Warning:
   If you want to change the name of the class, please set the name of the original script in the fourth parameter, 
   otherwise the type will be lost during serialization
   
   like this:
   
 
    [MovedFrom(false, "EasyBT.Script", "Assembly-CSharp", "OldActionName")]
    public class NewActionName : Act
    {
        public override void OnStart(BehaviorTree behaviorTree)
        {

        }

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {
            return TaskState.Failure;
        }

        public override void OnEnd(BehaviorTree behaviorTree)
        {
       
        }

    }
   
   

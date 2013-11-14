namespace InputControl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SlimDX.DirectInput;

    public class GamePad : AInputcontrol
    {
        private IList<GamepadDevice> padsList;
        private GamepadDevice device;
        private Joystick pad;

        public GamePad() : base()
        {
        }

        protected override void ProcessController()
        {
            ControllerState newState = new ControllerState();
            JoystickState joyState = new JoystickState();

            if (this.padsList == null || !this.padsList.Any())
            {
                this.padsList = this.GetGamePads();
                if (!this.padsList.Any()) return;
            }

            if (this.device == null)
            {
                this.device = this.padsList[0];
            }

            if (this.pad == null)
            {
                this.Acquire();
            }

            if (this.pad == null || this.pad.Poll().IsFailure || this.pad.GetCurrentState(ref joyState).IsFailure)
            {
                newState.Active = false;
            }
            else
            {
                //var objs = pad.GetObjects();
                newState.Active = true;
                newState.X = (joyState.X / 5000d);
                newState.Y = (joyState.Y / 5000d);
                newState.Buttons = joyState.GetButtons();
            }
            this.State = newState;

            if (this.OnUpdate != null)
            {
                this.OnUpdate.Invoke(this.State);
            }
        }

        private IList<GamepadDevice> GetGamePads()
        {
            IList<GamepadDevice> result = new List<GamepadDevice>();
            DirectInput dinput = new DirectInput();
            foreach (DeviceInstance di in dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                GamepadDevice dev = new GamepadDevice();
                dev.Guid = di.InstanceGuid;
                dev.Name = di.InstanceName;
                result.Add(dev);
            }
            return result;
        }

        private void Acquire()
        {
            DirectInput dinput = new DirectInput();

            this.pad = new Joystick(dinput, this.device.Guid);
            foreach (DeviceObjectInstance doi in this.pad.GetObjects(ObjectDeviceType.Axis))
            {
                this.pad.GetObjectPropertiesById((int)doi.ObjectType).SetRange(-5000, 5000);
            }

            this.pad.Properties.AxisMode = DeviceAxisMode.Absolute;
            //pad.SetCooperativeLevel(parent, (CooperativeLevel.Nonexclusive | CooperativeLevel.Background));
            this.pad.Acquire();
        }

        private class GamepadDevice
        {
            public Guid Guid { get; set; }
            public string Name { get; set; }
        }
    }


}

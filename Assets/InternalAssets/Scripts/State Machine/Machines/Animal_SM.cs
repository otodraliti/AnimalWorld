
using UnityEngine;
using StateManager;

    public class Animal_SM: MonoBehaviour
    {
        private StateMachine _SM;
        private Move_State _moveState;
        private Gather_State _gatherState;

        private void InitStates()
        {
            _SM = new StateMachine();
            _gatherState = new Gather_State(this.transform);
            _moveState = new Move_State();
            
            _SM.Initialize(_moveState);
        }
        
        private void Awake()
        {
            InitStates();
        }

        private void Update()
        {
            _SM.CurrentState.Update();
        }
        
        public void GatherState(GatherPoint gatherPoint)
        {
            _gatherState.SetGatherPoint(gatherPoint);
            _SM.ChangeState(_gatherState);
        }

        public void Reproduce()
        {
            Debug.Log(gameObject.name + " Reproduced=");
        }
    }
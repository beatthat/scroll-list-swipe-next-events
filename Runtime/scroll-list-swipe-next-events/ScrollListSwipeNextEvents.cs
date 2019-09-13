using BeatThat.TransformPathExt;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeatThat.ScrollLists.SwipeNextEvents
{
    public class ScrollListSwipeNextEvents : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
	{
		public bool m_debug;

		public UnityEvent recenter { get { return m_recenter; } }
		[SerializeField]private UnityEvent m_recenter = new UnityEvent();

		public UnityEvent scrollNext { get { return m_scrollNext; } }
		[SerializeField]private UnityEvent m_scrollNext = new UnityEvent();

		public UnityEvent scrollPrev { get { return m_scrollPrev; } }
		[SerializeField]private UnityEvent m_scrollPrev = new UnityEvent();

		#region IPointerDownHandler implementation
		public void OnPointerDown (PointerEventData eventData)
		{
			// in order to receive OnPointerUp event must implement IPointerDownHandler, even though we don't use the down event
//			Debug.LogError("[" + Time.frameCount + "][" + this.Path() + "] OnPointerDown");
		}
		#endregion

		#region IPointerUpHandler implementation

		public void OnPointerUp (PointerEventData eventData)
		{
			ChooseItemAndCenter();
		}

		#endregion

		private int lastChooseItemFrame { get; set; }

		private void ChooseItemAndCenter()
		{
			if (Time.frameCount == this.lastChooseItemFrame) {
				return; 
			}

			this.lastChooseItemFrame = Time.frameCount;

			var scrollVel = this.scrollRect.velocity.x;

			if(Mathf.Abs(scrollVel) < m_nextItemVelocityThreshold) {
				#if UNITY_EDITOR || DEBUG_UNSTRIP
				if(m_debug) {
					Debug.Log("[" + Time.frameCount + "][" + this.Path() 
						+ "] ChooseItemAndCenter scrollVel=" + scrollVel + " - will dispatch RECENTER");
				}
				#endif
				this.recenter.Invoke();
				return;
			}

			#if UNITY_EDITOR || DEBUG_UNSTRIP
			if(m_debug) {
				Debug.Log("[" + Time.frameCount + "][" + this.Path()
					+ "] ChooseItemAndCenter scrollVel=" + scrollVel + " - will dispatch "
					+ ((scrollVel > 0f)? "SCROLL_PREV": "SCROLL_NEXT"));
			}
			#endif

			var e = (scrollVel > 0f)? this.scrollPrev: this.scrollNext;
			e.Invoke();
		}

		private void OnScrollPosChanged(Vector2 pos)
		{
			if(Input.GetMouseButtonUp(0)) {
				ChooseItemAndCenter(); 
			}
		}

		void Start()
		{
			GetComponent<ScrollRect>().onValueChanged.AddListener(this.OnScrollPosChanged);
		}


		public float m_nextItemVelocityThreshold = 1f;

		private ScrollRect m_scrollRect;
		private ScrollRect scrollRect { get { return m_scrollRect?? (m_scrollRect = this.GetComponent<ScrollRect>()); } }

	}
}


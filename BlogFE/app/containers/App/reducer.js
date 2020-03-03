import produce from 'immer';
import { GET_CATEGORY_SUCCESS } from './constants';

export const initialState = {
  categories: [],
};

/* eslint-disable default-case, no-param-reassign */
const appReducer = (state = initialState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case GET_CATEGORY_SUCCESS:
        draft.categories = action.response.data;
        break;
    }
  });

export default appReducer;

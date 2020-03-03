import { createSelector } from 'reselect';
import { initialState } from './reducer';

/**
 * Direct selector to the postPage state domain
 */

const selectPostPageDomain = state => state.postPage || initialState;

/**
 * Other specific selectors
 */

/**
 * Default selector used by PostPage
 */

const makeSelectPostPage = () =>
  createSelector(
    selectPostPageDomain,
    substate => substate,
  );

export default makeSelectPostPage;
export { selectPostPageDomain };
